//==================================================================================================
// Unit tests for ExecuteTransitionHandler.
// Verifies task status transition execution.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Application.Handlers;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Workflows;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for ExecuteTransitionHandler.
/// </summary>
//==================================================================================================
public sealed class ExecuteTransitionHandlerTests
{
    private readonly ExecuteTransitionHandler handler = new();

    //==============================================================================================
    /// <summary>
    /// Handler_Should_ExecuteValidTransition
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_ExecuteValidTransition()
    {
        var task = new TaskItem { Status = TaskStatus.Created };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.InProgress,
            IsValid = true
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
        task.Status.Should().Be(TaskStatus.InProgress);
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_SkipInvalidTransition
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_SkipInvalidTransition()
    {
        var task = new TaskItem { Status = TaskStatus.Created };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.Completed,
            IsValid = false
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
        task.Status.Should().Be(TaskStatus.Created);
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_UpdateTaskStatus
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_UpdateTaskStatus()
    {
        var task = new TaskItem { Status = TaskStatus.InProgress };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.Completed,
            IsValid = true
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
        task.Status.Should().Be(TaskStatus.Completed);
    }
}
