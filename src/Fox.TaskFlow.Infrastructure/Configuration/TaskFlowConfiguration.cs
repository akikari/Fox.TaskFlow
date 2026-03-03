//==================================================================================================
// Application configuration model using ConfigKit for type-safe settings.
// Binds to appsettings.json configuration section.
//==================================================================================================

namespace Fox.TaskFlow.Infrastructure.Configuration;

//==================================================================================================
/// <summary>
/// Represents the application configuration settings.
/// </summary>
//==================================================================================================
public sealed class TaskFlowConfiguration
{
    //==============================================================================================
    /// <summary>
    /// Gets or sets the database configuration.
    /// </summary>
    //==============================================================================================
    public DatabaseConfiguration Database { get; set; } = new();

    //==============================================================================================
    /// <summary>
    /// Gets or sets the webhook configuration.
    /// </summary>
    //==============================================================================================
    public WebhookConfiguration Webhook { get; set; } = new();
}
