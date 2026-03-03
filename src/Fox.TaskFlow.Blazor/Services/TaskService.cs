//==================================================================================================
// HTTP service for communicating with the TaskFlow API.
// Handles all task-related HTTP operations with structured validation errors.
//==================================================================================================
using System.Text.Json;
using Fox.TaskFlow.Blazor.Exceptions;
using Fox.TaskFlow.Blazor.Models;

namespace Fox.TaskFlow.Blazor.Services;

//==================================================================================================
/// <summary>
/// Service for interacting with the TaskFlow API.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TaskService"/> class.
/// </remarks>
/// <param name="httpClientFactory">The HTTP client factory.</param>
/// <param name="jsonOptions">The JSON serializer options.</param>
//==================================================================================================
public sealed class TaskService(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonOptions)
{
    #region Fields

    private readonly HttpClient httpClient = httpClientFactory.CreateClient(nameof(TaskService));

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Gets all tasks from the API.
    /// </summary>
    /// <returns>A list of all tasks.</returns>
    //==============================================================================================
    public async Task<List<TaskDto>> GetAllTasksAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<TaskDto>>("api/tasks", jsonOptions);
        return response ?? [];
    }

    //==============================================================================================
    /// <summary>
    /// Gets a specific task by ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The requested task or null if not found.</returns>
    //==============================================================================================
    public async Task<TaskDto?> GetTaskByIdAsync(Guid id)
    {
        return await httpClient.GetFromJsonAsync<TaskDto>($"api/tasks/{id}", jsonOptions);
    }

    //==============================================================================================
    /// <summary>
    /// Gets filtered tasks based on optional criteria.
    /// </summary>
    /// <param name="searchTitle">Optional search term for task title.</param>
    /// <param name="status">Optional filter by task status.</param>
    /// <param name="dueBefore">Optional filter for tasks due before this date.</param>
    /// <param name="dueAfter">Optional filter for tasks due after this date.</param>
    /// <param name="showCompleted">Optional filter to show or hide completed tasks.</param>
    /// <returns>Filtered list of tasks with total count.</returns>
    //==============================================================================================
    public async Task<FilteredTasksDto> GetFilteredTasksAsync(string? searchTitle = null, int? status = null, DateTime? dueBefore = null, DateTime? dueAfter = null, bool? showCompleted = null)
    {
        var queryParams = new List<string>();

        if (!string.IsNullOrWhiteSpace(searchTitle))
        {
            queryParams.Add($"searchTitle={Uri.EscapeDataString(searchTitle)}");
        }

        if (status.HasValue)
        {
            queryParams.Add($"status={status.Value}");
        }

        if (dueBefore.HasValue)
        {
            queryParams.Add($"dueBefore={dueBefore.Value:yyyy-MM-dd}");
        }

        if (dueAfter.HasValue)
        {
            queryParams.Add($"dueAfter={dueAfter.Value:yyyy-MM-dd}");
        }

        if (showCompleted.HasValue)
        {
            queryParams.Add($"showCompleted={showCompleted.Value}");
        }

        var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
        var response = await httpClient.GetFromJsonAsync<FilteredTasksDto>($"api/tasks/filter{queryString}", jsonOptions);
        return response ?? new FilteredTasksDto();
    }

    //==============================================================================================
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="task">The task to create.</param>
    /// <returns>The created task.</returns>
    /// <exception cref="ValidationException">Thrown when validation errors occur.</exception>
    //==============================================================================================
    public async Task<TaskDto?> CreateTaskAsync(CreateTaskDto task)
    {
        var response = await httpClient.PostAsJsonAsync("api/tasks", task, jsonOptions);
        await HandleErrorResponseAsync(response);
        return await response.Content.ReadFromJsonAsync<TaskDto>(jsonOptions);
    }

    //==============================================================================================
    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="title">The new title.</param>
    /// <param name="description">The new description.</param>
    /// <param name="dueDate">The new due date.</param>
    /// <returns>The updated task.</returns>
    /// <exception cref="ValidationException">Thrown when validation errors occur.</exception>
    //==============================================================================================
    public async Task<TaskDto?> UpdateTaskAsync(Guid id, string title, string description, DateTime? dueDate)
    {
        var request = new { Title = title, Description = description, DueDate = dueDate };
        var response = await httpClient.PutAsJsonAsync($"api/tasks/{id}", request, jsonOptions);
        await HandleErrorResponseAsync(response);
        return await response.Content.ReadFromJsonAsync<TaskDto>(jsonOptions);
    }

    //==============================================================================================
    /// <summary>
    /// Transitions a task to a new status.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="targetStatus">The target status.</param>
    /// <returns>The updated task.</returns>
    /// <exception cref="ValidationException">Thrown when validation errors occur.</exception>
    //==============================================================================================
    public async Task<TaskDto?> TransitionTaskAsync(Guid id, TaskStatus targetStatus)
    {
        var request = new { TargetStatus = targetStatus };
        var response = await httpClient.PostAsJsonAsync($"api/tasks/{id}/transition", request, jsonOptions);
        await HandleErrorResponseAsync(response);
        return await response.Content.ReadFromJsonAsync<TaskDto>(jsonOptions);
    }

    //==============================================================================================
    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    //==============================================================================================
    public async Task DeleteTaskAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"api/tasks/{id}");
        response.EnsureSuccessStatusCode();
    }

    #endregion

    #region Private Methods

    //==============================================================================================
    /// <summary>
    /// Handles error responses from the API.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <exception cref="ValidationException">Thrown when validation errors occur.</exception>
    /// <exception cref="HttpRequestException">Thrown when the API returns an error.</exception>
    //==============================================================================================
    private async Task HandleErrorResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, jsonOptions);

        if (errorResponse?.Errors != null && errorResponse.Errors.Count > 0)
        {
            var messages = errorResponse.Errors.Select(e => e.Message).ToList();
            throw new ValidationException(messages);
        }

        throw new HttpRequestException($"API returned {(int)response.StatusCode}: {errorContent}");
    }

    #endregion
}
