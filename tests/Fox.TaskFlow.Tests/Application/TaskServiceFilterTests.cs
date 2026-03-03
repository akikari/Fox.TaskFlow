//==================================================================================================
// Unit tests for TaskService GetFilteredTasksAsync method.
// Verifies OptionKit filter logic with various criteria.
//==================================================================================================
using Fox.ChainKit;
using Fox.OptionKit;
using Fox.TaskFlow.Application.Configuration;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Application.Services;
using Fox.TaskFlow.Application.Validators;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Workflows;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for TaskService GetFilteredTasksAsync with OptionKit filters.
/// </summary>
//==================================================================================================
public sealed class TaskServiceFilterTests
{
    private readonly FakeTaskRepository repository;
    private readonly TaskService service;

    public TaskServiceFilterTests()
    {
        repository = new FakeTaskRepository();
        service = CreateTaskService();
    }

    //==============================================================================================
    /// <summary>
    /// GetFilteredTasksAsync_Should_ReturnAllTasks_WhenNoFiltersApplied
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetFilteredTasksAsync_Should_ReturnAllTasks_WhenNoFiltersApplied()
    {
        var request = new GetTasksRequest();

        var result = await service.GetFilteredTasksAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(3);
        result.Value.Tasks.Should().HaveCount(3);
    }

    //==============================================================================================
    /// <summary>
    /// GetFilteredTasksAsync_Should_FilterByTitle_WhenSearchTitleProvided
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetFilteredTasksAsync_Should_FilterByTitle_WhenSearchTitleProvided()
    {
        var request = new GetTasksRequest
        {
            SearchTitle = Option.Some("Task 1")
        };

        var result = await service.GetFilteredTasksAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.TotalCount.Should().Be(1);
        result.Value.Tasks[0].Title.Should().Contain("Task 1");
    }

    //==============================================================================================
    /// <summary>
    /// GetFilteredTasksAsync_Should_FilterByStatus_WhenStatusProvided
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetFilteredTasksAsync_Should_FilterByStatus_WhenStatusProvided()
    {
        var request = new GetTasksRequest
        {
            FilterByStatus = Option.Some(TaskStatus.Completed)
        };

        var result = await service.GetFilteredTasksAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Tasks.Should().OnlyContain(t => t.Status == TaskStatus.Completed);
    }

    //==============================================================================================
    /// <summary>
    /// GetFilteredTasksAsync_Should_FilterByDueBefore_WhenDueDateProvided
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetFilteredTasksAsync_Should_FilterByDueBefore_WhenDueDateProvided()
    {
        var futureDate = DateTime.Now.AddDays(5);
        var request = new GetTasksRequest
        {
            DueBefore = Option.Some(futureDate)
        };

        var result = await service.GetFilteredTasksAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Tasks.Should().OnlyContain(t => !t.DueDate.HasValue || t.DueDate.Value <= futureDate);
    }

    //==============================================================================================
    /// <summary>
    /// GetFilteredTasksAsync_Should_ExcludeCompleted_WhenShowCompletedIsFalse
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetFilteredTasksAsync_Should_ExcludeCompleted_WhenShowCompletedIsFalse()
    {
        var request = new GetTasksRequest
        {
            ShowCompleted = Option.Some(false)
        };

        var result = await service.GetFilteredTasksAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Tasks.Should().NotContain(t => t.Status == TaskStatus.Completed);
    }

    //==============================================================================================
    /// <summary>
    /// GetFilteredTasksAsync_Should_CombineMultipleFilters
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task GetFilteredTasksAsync_Should_CombineMultipleFilters()
    {
        var request = new GetTasksRequest
        {
            SearchTitle = Option.Some("Task"),
            FilterByStatus = Option.Some(TaskStatus.InProgress),
            ShowCompleted = Option.Some(false)
        };

        var result = await service.GetFilteredTasksAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Tasks.Should().OnlyContain(t => t.Title.Contains("Task") && t.Status == TaskStatus.InProgress);
    }

    private TaskService CreateTaskService()
    {
        var localizerFactory = new FakeStringLocalizerFactory();
        var messageProvider = new LocalizedMessageProvider(localizerFactory);
        var validationConfig = Options.Create(new ValidationConfiguration());
        var createValidator = new CreateTaskRequestValidator(validationConfig);
        var updateValidator = new UpdateTaskRequestValidator(validationConfig);
        var transitionValidator = new TransitionTaskRequestValidator();
        var transitionChain = new FakeChain();

        return new TaskService(repository, messageProvider, validationConfig, createValidator, updateValidator, transitionValidator, transitionChain);
    }

    private sealed class FakeTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> tasks =
        [
            new TaskItem { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1", Status = TaskStatus.Created, DueDate = DateTime.Now.AddDays(3) },
            new TaskItem { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2", Status = TaskStatus.InProgress, DueDate = DateTime.Now.AddDays(7) },
            new TaskItem { Id = Guid.NewGuid(), Title = "Task 3", Description = "Description 3", Status = TaskStatus.Completed, DueDate = null }
        ];

        public Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default) => Task.FromResult<IEnumerable<TaskItem>>(tasks);
        public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(tasks.FirstOrDefault(t => t.Id == id));
        public Task AddAsync(TaskItem task, CancellationToken cancellationToken = default) { tasks.Add(task); return Task.CompletedTask; }
        public Task UpdateAsync(TaskItem task, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task DeleteAsync(TaskItem task, CancellationToken cancellationToken = default) { tasks.Remove(task); return Task.CompletedTask; }
        public Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class FakeStringLocalizerFactory : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource) => new FakeStringLocalizer();
        public IStringLocalizer Create(string baseName, string location) => new FakeStringLocalizer();
    }

    private sealed class FakeStringLocalizer : IStringLocalizer
    {
        public LocalizedString this[string name] => new(name, name, false);
        public LocalizedString this[string name, params object[] arguments] => new(name, string.Format(name, arguments), false);
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];
    }

    private sealed class FakeChain : IChain<TaskTransitionContext>
    {
        public Task RunAsync(TaskTransitionContext context, CancellationToken cancellationToken = default)
        {
            context.Task.TransitionTo(context.TargetStatus);
            return Task.CompletedTask;
        }
    }
}
