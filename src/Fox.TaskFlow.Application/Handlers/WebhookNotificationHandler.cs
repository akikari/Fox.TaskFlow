//==================================================================================================
// Handler for sending webhook notifications after successful task transitions.
// Integrates WebhookNotificationService with ChainKit pipeline.
//==================================================================================================
using Fox.ChainKit;
using Fox.TaskFlow.Application.Interfaces;
using Fox.TaskFlow.Domain.Workflows;
using Microsoft.Extensions.Logging;

namespace Fox.TaskFlow.Application.Handlers;

//==================================================================================================
/// <summary>
/// Sends webhook notifications for task transitions.
/// </summary>
//==================================================================================================
public sealed class WebhookNotificationHandler : IHandler<TaskTransitionContext>
{
    #region Fields

    private readonly IWebhookNotificationService webhookService;
    private readonly ILogger<WebhookNotificationHandler> logger;

    #endregion

    #region Constructors

    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="WebhookNotificationHandler"/> class.
    /// </summary>
    /// <param name="webhookService">The webhook notification service.</param>
    /// <param name="logger">The logger.</param>
    //==============================================================================================
    public WebhookNotificationHandler(IWebhookNotificationService webhookService, ILogger<WebhookNotificationHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(webhookService);
        ArgumentNullException.ThrowIfNull(logger);

        this.webhookService = webhookService;
        this.logger = logger;
    }

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Handles webhook notification after task transition.
    /// </summary>
    /// <param name="context">The transition context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Continue result.</returns>
    //==============================================================================================
    public async Task<HandlerResult> HandleAsync(TaskTransitionContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(context.Task);

        if (!context.IsValid)
        {
            return HandlerResult.Continue;
        }

        try
        {
            await webhookService.NotifyTaskTransitionedAsync(context.Task, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP error sending webhook notification for task {TaskId} transition to {Status}", context.Task.Id, context.TargetStatus);
        }
        catch (TaskCanceledException ex)
        {
            logger.LogError(ex, "Webhook notification timeout for task {TaskId} transition to {Status}", context.Task.Id, context.TargetStatus);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Invalid operation sending webhook notification for task {TaskId} transition to {Status}", context.Task.Id, context.TargetStatus);
        }
#pragma warning disable CA1031
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error sending webhook notification for task {TaskId} transition to {Status}", context.Task.Id, context.TargetStatus);
        }
#pragma warning restore CA1031

        return HandlerResult.Continue;
    }

    #endregion
}
