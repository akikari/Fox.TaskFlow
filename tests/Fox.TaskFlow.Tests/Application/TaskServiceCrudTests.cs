//==================================================================================================
// Unit tests for TaskService CRUD operations with mocked dependencies.
// Tests business logic layer with Result pattern and Fox.MapKit integration.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Application.Configuration;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Application.Services;
using Fox.TaskFlow.Application.Validators;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Workflows;
using Fox.TaskFlow.Tests.Helpers;
using Microsoft.Extensions.Options;
using Moq;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for TaskService CRUD operations.
/// </summary>
//==================================================================================================
public sealed class TaskServiceCrudTests
{
    private readonly Mock<ITaskRepository> mockRepository;
    private readonly Mock<IChain<TaskTransitionContext>> mockChain;
    private readonly TaskService service;
    private readonly ValidationConfiguration config;

    public TaskServiceCrudTests()
    {
        mockRepository = new Mock<ITaskRepository>();
        mockChain = new Mock<IChain<TaskTransitionContext>>();
        config = new ValidationConfiguration();

        var messageProvider = new TestMessageProvider();
        var createValidator = new CreateTaskRequestValidator(Options.Create(config));
        var updateValidator = new UpdateTaskRequestValidator(Options.Create(config));
        var transitionValidator = new TransitionTaskRequestValidator();

        service = new TaskService(
            mockRepository.Object,
            messageProvider,
            Options.Create(config),
            createValidator,
            updateValidator,
            transitionValidator,
            mockChain.Object);
    }

    //==============================================================================================
    /// <summary>
    /// GetAllTasksAsync_Should_ReturnAllTasks_WhenTasksExist
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetAllTasksAsync_Should_ReturnAllTasks_WhenTasksExist()
    {
        var tasks = new List<TaskItem>
        {
            new(Guid.NewGuid(), "Task 1", "Description 1", null),
            new(Guid.NewGuid(), "Task 2", "Description 2", DateTime.UtcNow.AddDays(1))
        };

        mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(tasks);

        var result = await service.GetAllTasksAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value![0].Title.Should().Be("Task 1");
        result.Value[1].Title.Should().Be("Task 2");
    }

    //==============================================================================================
    /// <summary>
    /// GetAllTasksAsync_Should_ReturnEmptyList_WhenNoTasksExist
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetAllTasksAsync_Should_ReturnEmptyList_WhenNoTasksExist()
    {
        mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);

        var result = await service.GetAllTasksAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    //==============================================================================================
    /// <summary>
    /// GetTaskByIdAsync_Should_ReturnTask_WhenTaskExists
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetTaskByIdAsync_Should_ReturnTask_WhenTaskExists()
    {
        var taskId = Guid.NewGuid();
        var task = new TaskItem(taskId, "Test Task", "Test Description", null);

        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync(task);

        var result = await service.GetTaskByIdAsync(taskId);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.Should().Be(taskId);
        result.Value.Title.Should().Be("Test Task");
    }

    //==============================================================================================
    /// <summary>
    /// GetTaskByIdAsync_Should_ReturnFailure_WhenTaskNotFound
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetTaskByIdAsync_Should_ReturnFailure_WhenTaskNotFound()
    {
        var taskId = Guid.NewGuid();
        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync((TaskItem?)null);

        var result = await service.GetTaskByIdAsync(taskId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain(taskId.ToString());
    }

    //==============================================================================================
    /// <summary>
    /// CreateTaskAsync_Should_CreateTask_WhenRequestIsValid
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task CreateTaskAsync_Should_CreateTask_WhenRequestIsValid()
    {
        var request = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "Task description with enough characters",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        mockRepository.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await service.CreateTaskAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Title.Should().Be("New Task");
        result.Value.Description.Should().Be("Task description with enough characters");
        mockRepository.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    //==============================================================================================
    /// <summary>
    /// CreateTaskAsync_Should_ReturnFailure_WhenValidationFails
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task CreateTaskAsync_Should_ReturnFailure_WhenValidationFails()
    {
        var request = new CreateTaskRequest
        {
            Title = "AB",
            Description = "Short",
            DueDate = null
        };

        var result = await service.CreateTaskAsync(request);

        result.IsSuccess.Should().BeFalse();
        mockRepository.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    //==============================================================================================
    /// <summary>
    /// UpdateTaskAsync_Should_UpdateTask_WhenRequestIsValid
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task UpdateTaskAsync_Should_UpdateTask_WhenRequestIsValid()
    {
        var taskId = Guid.NewGuid();
        var existingTask = new TaskItem(taskId, "Original Title", "Original Description", null);
        var request = new UpdateTaskRequest
        {
            Title = "Updated Title",
            Description = "Updated description with enough characters",
            DueDate = DateTime.UtcNow.AddDays(3)
        };

        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync(existingTask);
        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await service.UpdateTaskAsync(taskId, request);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Title.Should().Be("Updated Title");
        result.Value.Description.Should().Be("Updated description with enough characters");
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    //==============================================================================================
    /// <summary>
    /// UpdateTaskAsync_Should_ReturnFailure_WhenValidationFails
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task UpdateTaskAsync_Should_ReturnFailure_WhenValidationFails()
    {
        var taskId = Guid.NewGuid();
        var request = new UpdateTaskRequest
        {
            Title = "",
            Description = "Valid description here",
            DueDate = null
        };

        var result = await service.UpdateTaskAsync(taskId, request);

        result.IsSuccess.Should().BeFalse();
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    //==============================================================================================
    /// <summary>
    /// UpdateTaskAsync_Should_ReturnFailure_WhenTaskNotFound
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task UpdateTaskAsync_Should_ReturnFailure_WhenTaskNotFound()
    {
        var taskId = Guid.NewGuid();
        var request = new UpdateTaskRequest
        {
            Title = "Valid Title",
            Description = "Valid description with enough characters",
            DueDate = null
        };

        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync((TaskItem?)null);

        var result = await service.UpdateTaskAsync(taskId, request);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain(taskId.ToString());
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    //==============================================================================================
    /// <summary>
    /// TransitionTaskAsync_Should_TransitionTask_WhenRequestIsValid
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task TransitionTaskAsync_Should_TransitionTask_WhenRequestIsValid()
    {
        var taskId = Guid.NewGuid();
        var task = new TaskItem(taskId, "Test Task", "Test Description", null);
        var request = new TransitionTaskRequest { TargetStatus = TaskStatus.InProgress };

        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync(task);
        mockChain.Setup(c => c.RunAsync(It.IsAny<TaskTransitionContext>(), It.IsAny<CancellationToken>()))
            .Callback<TaskTransitionContext, CancellationToken>((ctx, _) => ctx.IsValid = true)
            .Returns(Task.CompletedTask);
        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await service.TransitionTaskAsync(taskId, request);

        result.IsSuccess.Should().BeTrue();
        mockChain.Verify(c => c.RunAsync(It.IsAny<TaskTransitionContext>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    //==============================================================================================
    /// <summary>
    /// TransitionTaskAsync_Should_ReturnFailure_WhenValidationFails
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task TransitionTaskAsync_Should_ReturnFailure_WhenValidationFails()
    {
        var taskId = Guid.NewGuid();
        var request = new TransitionTaskRequest { TargetStatus = (TaskStatus)999 };

        var result = await service.TransitionTaskAsync(taskId, request);

        result.IsSuccess.Should().BeFalse();
        mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    //==============================================================================================
    /// <summary>
    /// TransitionTaskAsync_Should_ReturnFailure_WhenTransitionNotValid
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task TransitionTaskAsync_Should_ReturnFailure_WhenTransitionNotValid()
    {
        var taskId = Guid.NewGuid();
        var task = new TaskItem(taskId, "Test Task", "Test Description", null);
        var request = new TransitionTaskRequest { TargetStatus = TaskStatus.InProgress };

        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync(task);
        mockChain.Setup(c => c.RunAsync(It.IsAny<TaskTransitionContext>(), It.IsAny<CancellationToken>()))
            .Callback<TaskTransitionContext, CancellationToken>((ctx, _) => ctx.IsValid = false)
            .Returns(Task.CompletedTask);

        var result = await service.TransitionTaskAsync(taskId, request);

        result.IsSuccess.Should().BeFalse();
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    //==============================================================================================
    /// <summary>
    /// DeleteTaskAsync_Should_DeleteTask_WhenTaskExists
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task DeleteTaskAsync_Should_DeleteTask_WhenTaskExists()
    {
        var taskId = Guid.NewGuid();
        var task = new TaskItem(taskId, "Test Task", "Test Description", null);

        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync(task);
        mockRepository.Setup(r => r.DeleteAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await service.DeleteTaskAsync(taskId);

        result.IsSuccess.Should().BeTrue();
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    //==============================================================================================
    /// <summary>
    /// DeleteTaskAsync_Should_ReturnFailure_WhenTaskNotFound
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task DeleteTaskAsync_Should_ReturnFailure_WhenTaskNotFound()
    {
        var taskId = Guid.NewGuid();
        mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync((TaskItem?)null);

        var result = await service.DeleteTaskAsync(taskId);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain(taskId.ToString());
        mockRepository.Verify(r => r.DeleteAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
