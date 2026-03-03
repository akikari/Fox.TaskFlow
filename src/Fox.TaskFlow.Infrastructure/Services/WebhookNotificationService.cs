//==================================================================================================
// Webhook notification service implementation using RetryKit for resilient HTTP calls.
// Sends task notifications to external webhooks with automatic retry on failure.
//==================================================================================================
using System.Text;
using System.Text.Json;
using Fox.RetryKit;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Domain.Entities;
using Fox.TaskFlow.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fox.TaskFlow.Infrastructure.Services;

//==================================================================================================
/// <summary>
/// Implements webhook notifications with RetryKit resilience.
/// </summary>
//==================================================================================================
public sealed class WebhookNotificationService : IWebhookNotificationService
{
    #region Fields

    private readonly IHttpClientFactory httpClientFactory;
    private readonly WebhookConfiguration configuration;
    private readonly ILogger<WebhookNotificationService> logger;
    private readonly RetryPolicy retryPolicy;

    #endregion

    #region Constructors

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="WebhookNotificationService"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <param name="configuration">The webhook configuration.</param>
    /// <param name="logger">The logger.</param>
    //==============================================================================================
    public WebhookNotificationService(IHttpClientFactory httpClientFactory, IOptions<TaskFlowConfiguration> configuration, ILogger<WebhookNotificationService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientFactory);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logger);

        this.httpClientFactory = httpClientFactory;
        this.configuration = configuration.Value.Webhook;
        this.logger = logger;

        retryPolicy = RetryPolicy.Retry(this.configuration.MaxRetries, TimeSpan.FromMilliseconds(this.configuration.RetryDelayMs))
            .Handle<HttpRequestException>()
            .Handle<TaskCanceledException>()
            .OnRetry((exception, attempt, delay) =>
            {
                logger.LogWarning("Webhook notification retry attempt {Attempt} after {Delay}ms: {Message}", attempt, delay.TotalMilliseconds, exception.Message);
            });
    }

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Sends a notification when a task is created.
    /// </summary>
    /// <param name="task">The created task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    public async Task NotifyTaskCreatedAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        if (!configuration.Enabled || string.IsNullOrEmpty(configuration.Url))
        {
            return;
        }

        await retryPolicy.ExecuteAsync(async () =>
        {
            var payload = CreateWebhookPayload("TaskCreated", task, task.CreatedAt);
            await SendWebhookAsync(payload, cancellationToken);
        }, cancellationToken);
    }

    //==============================================================================================
    /// <summary>
    /// Sends a notification when a task status is transitioned.
    /// </summary>
    /// <param name="task">The transitioned task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    public async Task NotifyTaskTransitionedAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        if (!configuration.Enabled || string.IsNullOrEmpty(configuration.Url))
        {
            return;
        }

        await retryPolicy.ExecuteAsync(async () =>
        {
            var payload = CreateWebhookPayload("TaskTransitioned", task, task.UpdatedAt);
            await SendWebhookAsync(payload, cancellationToken);
        }, cancellationToken);
    }

    #endregion

    #region Private Methods

    //==============================================================================================
    /// <summary>
    /// Creates a webhook payload object.
    /// </summary>
    /// <param name="eventType">The event type (e.g., TaskCreated, TaskTransitioned).</param>
    /// <param name="task">The task entity.</param>
    /// <param name="timestamp">The event timestamp.</param>
    /// <returns>An anonymous object containing the webhook payload.</returns>
    //==============================================================================================
    private static object CreateWebhookPayload(string eventType, TaskItem task, DateTime timestamp)
    {
        return new
        {
            eventType,
            taskId = task.Id,
            title = task.Title,
            status = task.Status.ToString(),
            timestamp
        };
    }

    //==============================================================================================
    /// <summary>
    /// Sends the webhook HTTP request.
    /// </summary>
    /// <param name="payload">The payload object to serialize and send.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    private async Task SendWebhookAsync(object payload, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(configuration.Url, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Webhook notification sent successfully to {Url}", configuration.Url);
        }
    }

    #endregion
}
