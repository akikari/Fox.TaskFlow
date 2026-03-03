//==================================================================================================
// Unit tests for WebhookNotificationHandler.
// Verifies webhook notification integration with ChainKit pipeline.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Application.Handlers;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Domain.Workflows;
using Microsoft.Extensions.Logging;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for WebhookNotificationHandler.
/// </summary>
//==================================================================================================
public sealed class WebhookNotificationHandlerTests
{
    private readonly FakeWebhookService webhookService;
    private readonly WebhookNotificationHandler handler;

    public WebhookNotificationHandlerTests()
    {
        webhookService = new FakeWebhookService();
        var logger = new FakeLogger<WebhookNotificationHandler>();
        handler = new WebhookNotificationHandler(webhookService, logger);
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_SendNotification_WhenTransitionIsValid
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_SendNotification_WhenTransitionIsValid()
    {
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task", Status = TaskStatus.Created };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.InProgress,
            IsValid = true
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
        webhookService.NotificationSent.Should().BeTrue();
        webhookService.NotifiedTask.Should().Be(task);
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_SkipNotification_WhenTransitionIsInvalid
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_SkipNotification_WhenTransitionIsInvalid()
    {
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task", Status = TaskStatus.Created };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.InProgress,
            IsValid = false
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
        webhookService.NotificationSent.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// Handler_Should_ContinueOnException
    /// </summary>
    //==============================================================================================
    [Fact]
    public async Task Handler_Should_ContinueOnException()
    {
        webhookService.ShouldThrow = true;
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "Test Task", Status = TaskStatus.Created };
        var context = new TaskTransitionContext
        {
            Task = task,
            TargetStatus = TaskStatus.InProgress,
            IsValid = true
        };

        var result = await handler.HandleAsync(context);

        result.Should().Be(HandlerResult.Continue);
    }

    private sealed class FakeWebhookService : IWebhookNotificationService
    {
        public bool NotificationSent { get; private set; }
        public TaskItem? NotifiedTask { get; private set; }
        public bool ShouldThrow { get; set; }

        public Task NotifyTaskCreatedAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            if (ShouldThrow)
            {
                throw new InvalidOperationException("Webhook failed");
            }

            NotificationSent = true;
            NotifiedTask = task;
            return Task.CompletedTask;
        }

        public Task NotifyTaskTransitionedAsync(TaskItem task, CancellationToken cancellationToken = default)
        {
            if (ShouldThrow)
            {
                throw new InvalidOperationException("Webhook failed");
            }

            NotificationSent = true;
            NotifiedTask = task;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeLogger<T> : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { }
    }
}
