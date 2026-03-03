//==================================================================================================
// Unit tests for TaskWorkflow with execution and validation.
// Tests workflow transitions and error handling.
//==================================================================================================
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Workflows;

namespace Fox.TaskFlow.Tests.Domain;

//==================================================================================================
/// <summary>
/// Tests for TaskWorkflow.
/// </summary>
//==================================================================================================
public sealed class TaskWorkflowTests
{
    //==============================================================================================
    /// <summary>
    /// Workflow_Should_TransitionFromCreatedToInProgress
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Workflow_Should_TransitionFromCreatedToInProgress()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);

        TaskWorkflow.ExecuteTransition(task, TaskStatus.InProgress);

        task.Status.Should().Be(TaskStatus.InProgress);
    }

    //==============================================================================================
    /// <summary>
    /// Workflow_Should_FailForInvalidTransition
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Workflow_Should_FailForInvalidTransition()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);
        task.TransitionTo(TaskStatus.InProgress);
        task.TransitionTo(TaskStatus.Completed);

        var act = () => TaskWorkflow.ExecuteTransition(task, TaskStatus.InProgress);

        act.Should().Throw<InvalidOperationException>().WithMessage("*Cannot transition*");
    }

    //==============================================================================================
    /// <summary>
    /// Workflow_Should_AllowMultipleValidTransitions
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Workflow_Should_AllowMultipleValidTransitions()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);

        TaskWorkflow.ExecuteTransition(task, TaskStatus.InProgress);
        task.Status.Should().Be(TaskStatus.InProgress);

        TaskWorkflow.ExecuteTransition(task, TaskStatus.Completed);
        task.Status.Should().Be(TaskStatus.Completed);
    }

    //==============================================================================================
    /// <summary>
    /// Workflow_Should_AllowCancellationFromCreated
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Workflow_Should_AllowCancellationFromCreated()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);

        TaskWorkflow.ExecuteTransition(task, TaskStatus.Cancelled);

        task.Status.Should().Be(TaskStatus.Cancelled);
    }

    //==============================================================================================
    /// <summary>
    /// Workflow_Should_AllowBackTransitionFromInProgressToCreated
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Workflow_Should_AllowBackTransitionFromInProgressToCreated()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);
        task.TransitionTo(TaskStatus.InProgress);

        TaskWorkflow.ExecuteTransition(task, TaskStatus.Created);

        task.Status.Should().Be(TaskStatus.Created);
    }
}
