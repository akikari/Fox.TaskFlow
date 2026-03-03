//==================================================================================================
// Service implementation for task business operations with Fox.*Kit integration.
// Combines validation, mapping, repository operations, and ChainKit workflow.
//==================================================================================================
using Fox.ChainKit;
using Fox.MapKit;
using Fox.ResultKit;
using Fox.TaskFlow.Application.Configuration;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.DTOs.Responses;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Application.Validators;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Workflows;
using Microsoft.Extensions.Options;

namespace Fox.TaskFlow.Application.Services;

//==================================================================================================
/// <summary>
/// Service for task business operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TaskService"/> class.
/// </remarks>
/// <param name="repository">The task repository.</param>
/// <param name="messageProvider">The localized message provider.</param>
/// <param name="validationConfig">The validation configuration.</param>
/// <param name="createTaskValidator">The create task validator.</param>
/// <param name="updateTaskValidator">The update task validator.</param>
/// <param name="transitionTaskValidator">The transition task validator.</param>
/// <param name="transitionChain">The task transition chain.</param>
//==================================================================================================
public sealed class TaskService(
    ITaskRepository repository,
    LocalizedMessageProvider messageProvider,
    IOptions<ValidationConfiguration> validationConfig,
    CreateTaskRequestValidator createTaskValidator,
    UpdateTaskRequestValidator updateTaskValidator,
    TransitionTaskRequestValidator transitionTaskValidator,
    IChain<TaskTransitionContext> transitionChain) : ITaskService
{
    #region Fields

    private readonly ValidationConfiguration config = validationConfig.Value;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Gets all tasks.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing list of all tasks.</returns>
    //==============================================================================================
    public async Task<Result<List<TaskResponse>>> GetAllTasksAsync(CancellationToken cancellationToken = default)
    {
        var tasks = await repository.GetAllAsync(cancellationToken);
        var responses = tasks.Select(task => Mapper.Map<TaskItem, TaskResponse>(task)).ToList();
        return Result<List<TaskResponse>>.Success(responses);
    }

    //==============================================================================================
    /// <summary>
    /// Gets a specific task by ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the requested task.</returns>
    //==============================================================================================
    public async Task<Result<TaskResponse>> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
        {
            return Result<TaskResponse>.Failure($"Task with ID {id} not found");
        }

        var response = Mapper.Map<TaskItem, TaskResponse>(task);
        return Result<TaskResponse>.Success(response);
    }

    //==============================================================================================
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="request">The task creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created task.</returns>
    //==============================================================================================
    public async Task<Result<TaskResponse>> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = createTaskValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            var localizedErrors = validationResult.Errors.Select(error => GetLocalizedValidationMessage(error.Message));
            return Result<TaskResponse>.Failure(string.Join("; ", localizedErrors));
        }

        var task = new TaskItem(Guid.NewGuid(), request.Title!, request.Description!, request.DueDate);

        await repository.AddAsync(task, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var response = Mapper.Map<TaskItem, TaskResponse>(task);
        return Result<TaskResponse>.Success(response);
    }

    //==============================================================================================
    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="request">The task update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the updated task.</returns>
    //==============================================================================================
    public async Task<Result<TaskResponse>> UpdateTaskAsync(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = updateTaskValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            var localizedErrors = validationResult.Errors.Select(error => GetLocalizedValidationMessage(error.Message));
            return Result<TaskResponse>.Failure(string.Join("; ", localizedErrors));
        }

        var task = await repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
        {
            return Result<TaskResponse>.Failure(messageProvider.GetFormatMessage(ValidationMessageKeys.TaskNotFound, id));
        }

        task.Update(request.Title!, request.Description!, request.DueDate);

        await repository.UpdateAsync(task, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var response = Mapper.Map<TaskItem, TaskResponse>(task);
        return Result<TaskResponse>.Success(response);
    }

    //==============================================================================================
    /// <summary>
    /// Transitions a task to a new status.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="request">The transition request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the updated task.</returns>
    //==============================================================================================
    public async Task<Result<TaskResponse>> TransitionTaskAsync(Guid id, TransitionTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var validationResult = transitionTaskValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            var localizedErrors = validationResult.Errors.Select(error => messageProvider.GetMessage(error.Message));
            return Result<TaskResponse>.Failure(string.Join("; ", localizedErrors));
        }

        var task = await repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
        {
            return Result<TaskResponse>.Failure($"Task with ID {id} not found");
        }

        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = request.TargetStatus
        };

        await transitionChain.RunAsync(context, cancellationToken);

        if (!context.IsValid)
        {
            return Result<TaskResponse>.Failure($"Cannot transition from {task.Status} to {request.TargetStatus}");
        }

        await repository.UpdateAsync(task, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        var response = Mapper.Map<TaskItem, TaskResponse>(task);
        return Result<TaskResponse>.Success(response);
    }

    //==============================================================================================
    /// <summary>
    /// Gets filtered tasks based on the request criteria.
    /// </summary>
    /// <param name="request">The filter request with optional criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing filtered tasks.</returns>
    //==============================================================================================
    public async Task<Result<GetTasksResponse>> GetFilteredTasksAsync(GetTasksRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var allTasks = await repository.GetAllAsync(cancellationToken);
        var query = allTasks.AsQueryable();

        query = request.SearchTitle.Match(
            searchTerm => query.Where(t => t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)),
            () => query);

        query = request.FilterByStatus.Match(
            status => query.Where(t => t.Status == status),
            () => query);

        query = request.DueBefore.Match(
            date => query.Where(t => t.DueDate.HasValue && t.DueDate.Value <= date),
            () => query);

        query = request.DueAfter.Match(
            date => query.Where(t => t.DueDate.HasValue && t.DueDate.Value >= date),
            () => query);

        query = request.ShowCompleted.Match(
            show => show ? query : query.Where(t => t.Status != TaskStatus.Completed),
            () => query);

        var filteredTasks = query.ToList();
        var responses = filteredTasks.Select(task => Mapper.Map<TaskItem, TaskResponse>(task)).ToList();

        return Result<GetTasksResponse>.Success(new GetTasksResponse
        {
            Tasks = responses,
            TotalCount = responses.Count
        });
    }

    //==============================================================================================
    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    //==============================================================================================
    public async Task<Result> DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await repository.GetByIdAsync(id, cancellationToken);
        if (task == null)
        {
            return Result.Failure($"Task with ID {id} not found");
        }

        await repository.DeleteAsync(task, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    #endregion

    #region Private Methods

    //==============================================================================================
    /// <summary>
    /// Gets the localized validation message for a given key.
    /// </summary>
    /// <param name="key">The validation message key.</param>
    /// <returns>The localized message with parameters if applicable.</returns>
    //==============================================================================================
    private string GetLocalizedValidationMessage(string key)
    {
        return key switch
        {
            ValidationMessageKeys.TitleMinLength => messageProvider.GetFormatMessage(key, config.TitleMinLength),
            ValidationMessageKeys.TitleMaxLength => messageProvider.GetFormatMessage(key, config.TitleMaxLength),
            ValidationMessageKeys.DescriptionMinLength => messageProvider.GetFormatMessage(key, config.DescriptionMinLength),
            ValidationMessageKeys.DescriptionMaxLength => messageProvider.GetFormatMessage(key, config.DescriptionMaxLength),
            _ => messageProvider.GetMessage(key)
        };
    }

    #endregion
}
