//==================================================================================================
// Service interface for task business operations with ResultKit responses.
// Replaces MediatR handlers with direct service calls.
//==================================================================================================
using Fox.ResultKit;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.DTOs.Responses;

namespace Fox.TaskFlow.Application.Interfaces;

//==================================================================================================
/// <summary>
/// Service for task business operations.
/// </summary>
//==================================================================================================
public interface ITaskService
{
    //==============================================================================================
    /// <summary>
    /// Gets all tasks.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing list of all tasks.</returns>
    //==============================================================================================
    Task<Result<List<TaskResponse>>> GetAllTasksAsync(CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Gets filtered tasks based on the request criteria.
    /// </summary>
    /// <param name="request">The filter request with optional criteria.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing filtered tasks.</returns>
    //==============================================================================================
    Task<Result<GetTasksResponse>> GetFilteredTasksAsync(GetTasksRequest request, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Gets a specific task by ID.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the requested task.</returns>
    //==============================================================================================
    Task<Result<TaskResponse>> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="request">The task creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the created task.</returns>
    //==============================================================================================
    Task<Result<TaskResponse>> CreateTaskAsync(CreateTaskRequest request, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="request">The task update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the updated task.</returns>
    //==============================================================================================
    Task<Result<TaskResponse>> UpdateTaskAsync(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Transitions a task to a new status.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="request">The transition request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result containing the updated task.</returns>
    //==============================================================================================
    Task<Result<TaskResponse>> TransitionTaskAsync(Guid id, TransitionTaskRequest request, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Deletes a task.
    /// </summary>
    /// <param name="id">The task ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    //==============================================================================================
    Task<Result> DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default);
}
