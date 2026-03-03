//==================================================================================================
// Individual error detail from API with localized message.
// Represents a single validation or business logic error.
//==================================================================================================

namespace Fox.TaskFlow.Blazor.Models;

//==================================================================================================
/// <summary>
/// Individual error detail from API with localized message.
/// </summary>
//==================================================================================================
internal sealed class ApiErrorDetail
{
    //==============================================================================================
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    //==============================================================================================
    public string Message { get; set; } = string.Empty;
}
