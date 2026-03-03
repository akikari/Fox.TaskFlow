//==================================================================================================
// Unit tests for TaskItem domain entity with nullable DateTime and workflow validation.
// Tests creation, updates, and workflow transitions.
//==================================================================================================
using Fox.TaskFlow.Domain.Entities;

namespace Fox.TaskFlow.Tests.Domain;

//==================================================================================================
/// <summary>
/// Tests for TaskItem domain entity.
/// </summary>
//==================================================================================================
public sealed class TaskItemTests
{
    //==============================================================================================
    /// <summary>
    /// TaskItem_Should_CreateWithCorrectInitialState
    /// </summary>
    //==============================================================================================
    [Fact]
    public void TaskItem_Should_CreateWithCorrectInitialState()
    {
        var id = Guid.NewGuid();
        var title = "Test Task";
        var description = "Test Description";
        var dueDate = DateTime.UtcNow.AddDays(7);

        var task = new TaskItem(id, title, description, dueDate);

        task.Id.Should().Be(id);
        task.Title.Should().Be(title);
        task.Description.Should().Be(description);
        task.DueDate.Should().Be(dueDate);
        task.Status.Should().Be(TaskStatus.Created);
        task.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    //==============================================================================================
    /// <summary>
    /// TaskItem_Should_CreateWithoutDueDate
    /// </summary>
    //==============================================================================================
    [Fact]
    public void TaskItem_Should_CreateWithoutDueDate()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);

        task.DueDate.HasValue.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// TaskItem_Should_UpdateProperties
    /// </summary>
    //==============================================================================================
    [Fact]
    public void TaskItem_Should_UpdateProperties()
    {
        var task = new TaskItem(Guid.NewGuid(), "Old Title", "Old Description", null);
        var newDueDate = DateTime.UtcNow.AddDays(5);

        task.Update("New Title", "New Description", newDueDate);

        task.Title.Should().Be("New Title");
        task.Description.Should().Be("New Description");
        task.DueDate.Should().Be(newDueDate);
    }

    //==============================================================================================
    /// <summary>
    /// TaskItem_Should_AllowTransitionFromCreatedToInProgress
    /// </summary>
    //==============================================================================================
    [Fact]
    public void TaskItem_Should_AllowTransitionFromCreatedToInProgress()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);

        task.CanTransitionTo(TaskStatus.InProgress).Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// TaskItem_Should_AllowTransitionFromInProgressToCompleted
    /// </summary>
    //==============================================================================================
    [Fact]
    public void TaskItem_Should_AllowTransitionFromInProgressToCompleted()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);
        task.TransitionTo(TaskStatus.InProgress);

        task.CanTransitionTo(TaskStatus.Completed).Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// TaskItem_Should_NotAllowTransitionFromCompletedToInProgress
    /// </summary>
    //==============================================================================================
    [Fact]
    public void TaskItem_Should_NotAllowTransitionFromCompletedToInProgress()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);
        task.TransitionTo(TaskStatus.InProgress);
        task.TransitionTo(TaskStatus.Completed);

        task.CanTransitionTo(TaskStatus.InProgress).Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// TaskItem_Should_UpdateUpdatedAtOnTransition
    /// </summary>
    //==============================================================================================
    [Fact]
    public void TaskItem_Should_UpdateUpdatedAtOnTransition()
    {
        var task = new TaskItem(Guid.NewGuid(), "Test", "Description", null);
        var originalUpdatedAt = task.UpdatedAt;

        Thread.Sleep(10);
        task.TransitionTo(TaskStatus.InProgress);

        task.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }
}
