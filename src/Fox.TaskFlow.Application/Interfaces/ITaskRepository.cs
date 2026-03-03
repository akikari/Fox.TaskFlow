//==================================================================================================
// Repository interface for task persistence operations.
// Follows Repository pattern for domain entity access.
//==================================================================================================
using Fox.TaskFlow.Domain.Entities;

namespace Fox.TaskFlow.Application.Interfaces;

//==================================================================================================
/// <summary>
/// Defines operations for task persistence.
/// </summary>
//==================================================================================================
public interface ITaskRepository
{
    //==============================================================================================
    /// <summary>
    /// Retrieves a task by its unique identifier.
    /// </summary>
    /// <param name="id">The task identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The task if found; otherwise, null.</returns>
    //==============================================================================================
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of all tasks.</returns>
    //==============================================================================================
    Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Adds a new task to the repository.
    /// </summary>
    /// <param name="task">The task to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    Task AddAsync(TaskItem task, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Updates an existing task in the repository.
    /// </summary>
    /// <param name="task">The task to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Deletes a task from the repository.
    /// </summary>
    /// <param name="task">The task to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    Task DeleteAsync(TaskItem task, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Saves all changes to the underlying data store.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
