//==================================================================================================
// Handler for validating task status transitions.
// Checks if the requested transition is allowed based on workflow rules.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Domain.Workflows;

namespace Fox.TaskFlow.Application.Handlers;

//==================================================================================================
/// <summary>
/// Validates task status transitions according to workflow rules.
/// </summary>
//==================================================================================================
public sealed class ValidateTransitionHandler : IHandler<TaskTransitionContext>
{
    //==============================================================================================
    /// <summary>
    /// Handles the validation of task transition.
    /// </summary>
    /// <param name="context">The transition context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Continue if transition is valid; Stop otherwise.</returns>
    //==============================================================================================
    public Task<HandlerResult> HandleAsync(TaskTransitionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.Task);

        if (!context.Task.CanTransitionTo(context.TargetStatus))
        {
            context.IsValid = false;
            return Task.FromResult(HandlerResult.Stop);
        }

        context.IsValid = true;
        return Task.FromResult(HandlerResult.Continue);
    }
}
