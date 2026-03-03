//==================================================================================================
// Interface for webhook notification service with RetryKit integration.
// Defines contract for sending task notifications to external webhooks.
//==================================================================================================
using Fox.TaskFlow.Domain.Entities;

namespace Fox.TaskFlow.Application.Interfaces;

//==================================================================================================
/// <summary>
/// Defines operations for sending webhook notifications.
/// </summary>
//==================================================================================================
public interface IWebhookNotificationService
{
    //==============================================================================================
    /// <summary>
    /// Sends a notification when a task is created.
    /// </summary>
    /// <param name="task">The created task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    Task NotifyTaskCreatedAsync(TaskItem task, CancellationToken cancellationToken = default);

    //==============================================================================================
    /// <summary>
    /// Sends a notification when a task status is transitioned.
    /// </summary>
    /// <param name="task">The transitioned task.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    //==============================================================================================
    Task NotifyTaskTransitionedAsync(TaskItem task, CancellationToken cancellationToken = default);
}
