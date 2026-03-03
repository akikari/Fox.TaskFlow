//==================================================================================================
// REST API controller for task operations with service layer and localized error responses.
// Exposes endpoints with server-side localized JSON error messages.
//==================================================================================================
using Fox.OptionKit;
using Fox.ResultKit;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.DTOs.Responses;
using Fox.TaskFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskStatus = Fox.TaskFlow.Domain.Enums.TaskStatus;

namespace Fox.TaskFlow.Api.Controllers;

//==================================================================================================
/// <summary>
/// API controller for managing tasks.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TasksController"/> class.
/// </remarks>
/// <param name="taskService">The task service.</param>
//==================================================================================================
[ApiController]
[Route("api/[controller]")]
public sealed class TasksController(ITaskService taskService) : ControllerBase
{
    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Gets all tasks.
    /// </summary>
    /// <returns>A list of all tasks.</returns>
    //==============================================================================================
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await taskService.GetAllTasksAsync();
        return result.Match<List<TaskResponse>, IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(CreateErrorResponse(error)));
    }

    //==============================================================================================
    /// <summary>
    /// Gets a specific task by ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>The requested task.</returns>
    //==============================================================================================
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await taskService.GetTaskByIdAsync(id);
        return result.Match<TaskResponse, IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(CreateErrorResponse(error)));
    }

    //==============================================================================================
    /// <summary>
    /// Gets filtered tasks based on query parameters.
    /// </summary>
    /// <param name="searchTitle">Optional search term for task title.</param>
    /// <param name="status">Optional filter by task status.</param>
    /// <param name="dueBefore">Optional filter for tasks due before this date.</param>
    /// <param name="dueAfter">Optional filter for tasks due after this date.</param>
    /// <param name="showCompleted">Optional filter to show or hide completed tasks.</param>
    /// <returns>Filtered list of tasks.</returns>
    //==============================================================================================
    [HttpGet("filter")]
    public async Task<IActionResult> GetFiltered([FromQuery] string? searchTitle, [FromQuery] int? status, [FromQuery] DateTime? dueBefore, [FromQuery] DateTime? dueAfter, [FromQuery] bool? showCompleted)
    {
        var request = new GetTasksRequest
        {
            SearchTitle = searchTitle.ToOption(),
            FilterByStatus = ((TaskStatus?)status).ToOption(),
            DueBefore = dueBefore.ToOption(),
            DueAfter = dueAfter.ToOption(),
            ShowCompleted = showCompleted.ToOption()
        };

        var result = await taskService.GetFilteredTasksAsync(request);
        return result.Match<GetTasksResponse, IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(CreateErrorResponse(error)));
    }

    //==============================================================================================
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="request">The task creation request.</param>
    /// <returns>The created task.</returns>
    //==============================================================================================
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var result = await taskService.CreateTaskAsync(request);
        return result.Match<TaskResponse, IActionResult>(
            onSuccess: value => CreatedAtAction(nameof(GetById), new { id = value.Id }, value),
            onFailure: error => BadRequest(CreateErrorResponse(error)));
    }

    //==============================================================================================
    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="request">The task update request.</param>
    /// <returns>The updated task.</returns>
    //==============================================================================================
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var result = await taskService.UpdateTaskAsync(id, request);
        return result.Match<TaskResponse, IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(CreateErrorResponse(error)));
    }

    //==============================================================================================
    /// <summary>
    /// Transitions a task to a new status.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="request">The transition request.</param>
    /// <returns>The updated task.</returns>
    //==============================================================================================
    [HttpPost("{id}/transition")]
    public async Task<IActionResult> Transition(Guid id, [FromBody] TransitionTaskRequest request)
    {
        var result = await taskService.TransitionTaskAsync(id, request);
        return result.Match<TaskResponse, IActionResult>(
            onSuccess: value => Ok(value),
            onFailure: error => BadRequest(CreateErrorResponse(error)));
    }

    //==============================================================================================
    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <returns>No content on success.</returns>
    //==============================================================================================
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await taskService.DeleteTaskAsync(id);
        return result.Match<IActionResult>(
            onSuccess: () => NoContent(),
            onFailure: error => NotFound(CreateErrorResponse(error)));
    }

    #endregion

    #region Private Methods

    //==============================================================================================
    /// <summary>
    /// Creates structured error response with localized messages only.
    /// </summary>
    //==============================================================================================
    private static object CreateErrorResponse(string errorString)
    {
        var errors = errorString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(message => new { message }).ToList();

        return new { errors };
    }

    #endregion
}
