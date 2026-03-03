//==================================================================================================
// Database configuration settings for EF Core.
// Defines connection string, timeout, and logging options.
//==================================================================================================

namespace Fox.TaskFlow.Infrastructure.Configuration;

//==================================================================================================
/// <summary>
/// Represents database configuration settings.
/// </summary>
//==================================================================================================
public sealed class DatabaseConfiguration
{
    //==============================================================================================
    /// <summary>
    /// Gets or sets the connection string for the SQLite database.
    /// </summary>
    //==============================================================================================
    public string ConnectionString { get; set; } = "Data Source=taskflow.db";

    //==============================================================================================
    /// <summary>
    /// Gets or sets the command timeout in seconds.
    /// </summary>
    //==============================================================================================
    public int CommandTimeoutSeconds { get; set; } = 30;

    //==============================================================================================
    /// <summary>
    /// Gets or sets whether to enable detailed error logging for database operations.
    /// </summary>
    //==============================================================================================
    public bool EnableDetailedErrors { get; set; } = false;

    //==============================================================================================
    /// <summary>
    /// Gets or sets whether to enable sensitive data logging (e.g., parameter values).
    /// </summary>
    //==============================================================================================
    public bool EnableSensitiveDataLogging { get; set; } = false;
}
