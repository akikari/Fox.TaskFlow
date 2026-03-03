//==================================================================================================
// Repository implementation for task persistence using Entity Framework Core.
// Maps between domain models and database entities using MapKit.
//==================================================================================================
using Fox.MapKit;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Infrastructure.Data;
using Fox.TaskFlow.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fox.TaskFlow.Infrastructure.Repositories;

//==================================================================================================
/// <summary>
/// Implements task repository using Entity Framework Core and SQLite.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TaskRepository"/> class.
/// </remarks>
/// <param name="context">The database context.</param>
//==================================================================================================
public sealed class TaskRepository(TaskFlowDbContext context) : ITaskRepository
{
    //==============================================================================================
    /// <summary>
    /// Retrieves a task by its unique identifier.
    /// </summary>
    /// <param name="id">The task identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The task if found; otherwise, null.</returns>
    //==============================================================================================
    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Tasks.FindAsync([id], cancellationToken);
        return entity == null ? null : Mapper.Map<TaskEntity, TaskItem>(entity);
    }

    //==============================================================================================
    /// <summary>
    /// Retrieves all tasks.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of all tasks.</returns>
    //==============================================================================================
    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await context.Tasks.ToListAsync(cancellationToken);
        return [.. entities.Select(e => Mapper.Map<TaskEntity, TaskItem>(e))];
    }

    //==============================================================================================
    /// <summary>
    /// Adds a new task to the repository.
    /// </summary>
    /// <param name="task">The task to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        var entity = Mapper.Map<TaskItem, TaskEntity>(task);
        await context.Tasks.AddAsync(entity, cancellationToken);
    }

    //==============================================================================================
    /// <summary>
    /// Updates an existing task in the repository.
    /// </summary>
    /// <param name="task">The task to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>
    /// Uses Mapper.MapToExisting to update tracked entity properties while preserving EF Core
    /// change tracking. This approach is more efficient than creating a new entity instance.
    /// </remarks>
    //==============================================================================================
    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);

        var entity = await context.Tasks.FindAsync([task.Id], cancellationToken);
        if (entity != null)
        {
            Mapper.MapToExisting(task, entity);
        }
    }

    //==============================================================================================
    /// <summary>
    /// Deletes a task from the repository.
    /// </summary>
    /// <param name="task">The task to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    public async Task DeleteAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);

        var entity = await context.Tasks.FindAsync([task.Id], cancellationToken);
        if (entity != null)
        {
            context.Tasks.Remove(entity);
        }
    }

    //==============================================================================================
    /// <summary>
    /// Saves all changes to the underlying data store.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}
