//==================================================================================================
// Unit tests for ValidateTransitionHandler.
// Verifies workflow transition validation rules.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Application.Handlers;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Workflows;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for ValidateTransitionHandler.
/// </summary>
//==================================================================================================
public sealed class ValidateTransitionHandlerTests
{
    private readonly ValidateTransitionHandler handler;

    public ValidateTransitionHandlerTests()
    {
        handler = new ValidateTransitionHandler();
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_AllowValidTransition
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_AllowValidTransition()
    {
        var task = new TaskItem { Status = TaskStatus.Created };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.InProgress
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
        context.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_RejectInvalidTransition
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_RejectInvalidTransition()
    {
        var task = new TaskItem { Status = TaskStatus.Completed };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.Created
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Stop);
        context.IsValid.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_AllowCreatedToInProgress
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_AllowCreatedToInProgress()
    {
        var task = new TaskItem { Status = TaskStatus.Created };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.InProgress
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_AllowInProgressToCompleted
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_AllowInProgressToCompleted()
    {
        var task = new TaskItem { Status = TaskStatus.InProgress };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.Completed
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
    }
}
