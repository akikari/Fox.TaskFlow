//==================================================================================================
// Handler for executing validated task status transitions.
// Performs the actual state change after validation passes.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Domain.Workflows;

namespace Fox.TaskFlow.Application.Handlers;

//==================================================================================================
/// <summary>
/// Executes the actual task status transition after validation.
/// </summary>
//==================================================================================================
public sealed class ExecuteTransitionHandler : IHandler<TaskTransitionContext>
{
    //==============================================================================================
    /// <summary>
    /// Handles the execution of task transition.
    /// </summary>
    /// <param name="context">The transition context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Continue result.</returns>
    //==============================================================================================
    public Task<HandlerResult> HandleAsync(TaskTransitionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.Task);

        if (context.IsValid)
        {
            context.Task.TransitionTo(context.TargetStatus);
        }

        return Task.FromResult(HandlerResult.Continue);
    }
}
