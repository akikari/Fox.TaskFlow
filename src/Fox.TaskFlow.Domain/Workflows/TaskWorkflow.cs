//==================================================================================================
// Implements task workflow transitions with validation and state management.
// Provides methods for transitioning tasks through their lifecycle states.
//==================================================================================================
using Fox.TaskFlow.Domain.Entities;

namespace Fox.TaskFlow.Domain.Workflows;

//==================================================================================================
/// <summary>
/// Defines the workflow for task status transitions.
/// </summary>
//==================================================================================================
public sealed class TaskWorkflow
{
    //==============================================================================================
    /// <summary>
    /// Executes a status transition for a task with validation.
    /// </summary>
    /// <param name="task">The task to transition.</param>
    /// <param name="targetStatus">The target status to transition to.</param>
    /// <returns>The transitioned task.</returns>
    /// <exception cref="InvalidOperationException">Thrown when transition is not allowed.</exception>
    //==============================================================================================
    public static TaskItem ExecuteTransition(TaskItem task, TaskStatus targetStatus)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (!task.CanTransitionTo(targetStatus))
        {
            throw new InvalidOperationException($"Cannot transition from {task.Status} to {targetStatus}");
        }

        task.TransitionTo(targetStatus);
        return task;
    }

    //==============================================================================================
    /// <summary>
    /// Validates whether a transition is allowed without executing it.
    /// </summary>
    /// <param name="task">The task to validate.</param>
    /// <param name="targetStatus">The target status.</param>
    /// <returns><c>true</c> if transition is valid; otherwise, <c>false</c>.</returns>
    //==============================================================================================
    public static bool CanTransition(TaskItem task, TaskStatus targetStatus)
    {
        ArgumentNullException.ThrowIfNull(task);

        return task.CanTransitionTo(targetStatus);
    }
}
