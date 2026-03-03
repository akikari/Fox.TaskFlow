//==================================================================================================
// Webhook configuration settings for external notifications.
// Defines webhook URL, retry policy, and timeout settings.
//==================================================================================================

namespace Fox.TaskFlow.Infrastructure.Configuration;

//==================================================================================================
/// <summary>
/// Represents webhook configuration settings.
/// </summary>
//==================================================================================================
public sealed class WebhookConfiguration
{
    //==============================================================================================
    /// <summary>
    /// Gets or sets the webhook URL for task notifications.
    /// </summary>
    //==============================================================================================
    public string Url { get; set; } = string.Empty;

    //==============================================================================================
    /// <summary>
    /// Gets or sets whether webhook notifications are enabled.
    /// </summary>
    //==============================================================================================
    public bool Enabled { get; set; }

    //==============================================================================================
    /// <summary>
    /// Gets or sets the maximum number of retry attempts.
    /// </summary>
    //==============================================================================================
    public int MaxRetries { get; set; } = 3;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the retry delay in milliseconds.
    /// </summary>
    //==============================================================================================
    public int RetryDelayMs { get; set; } = 1000;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the webhook timeout in seconds.
    /// </summary>
    //==============================================================================================
    public int TimeoutSeconds { get; set; } = 30;
}
