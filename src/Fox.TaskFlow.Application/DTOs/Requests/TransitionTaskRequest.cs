//==================================================================================================
// Request DTO for transitioning a task to a new workflow status.
// Used in CQRS TransitionTaskCommand with ChainKit workflow.
//==================================================================================================

namespace Fox.TaskFlow.Application.DTOs.Requests;

//==================================================================================================
/// <summary>
/// Request model for transitioning a task to a new status.
/// </summary>
//==================================================================================================
public sealed record TransitionTaskRequest
{
    //==============================================================================================
    /// <summary>
    /// Gets the target status to transition to.
    /// </summary>
    //==============================================================================================
    public required TaskStatus TargetStatus { get; init; }
}
