//==================================================================================================
// Context for task status transition chain processing.
// Carries task state and transition information through the handler chain.
//==================================================================================================
using Fox.TaskFlow.Domain.Entities;

namespace Fox.TaskFlow.Domain.Workflows;

//==================================================================================================
/// <summary>
/// Context for task transition chain processing.
/// </summary>
//==================================================================================================
public sealed class TaskTransitionContext
{
    //==============================================================================================
    /// <summary>
    /// Gets or sets the task being transitioned.
    /// </summary>
    //==============================================================================================
    public TaskItem Task { get; set; } = default!;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the target status for the transition.
    /// </summary>
    //==============================================================================================
    public TaskStatus TargetStatus { get; set; }

    //==============================================================================================
    /// <summary>
    /// Gets or sets a value indicating whether the transition is valid.
    /// </summary>
    //==============================================================================================
    public bool IsValid { get; set; }
}
