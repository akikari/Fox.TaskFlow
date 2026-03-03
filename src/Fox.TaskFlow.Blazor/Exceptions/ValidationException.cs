//==================================================================================================
// Custom exception for validation errors with localized messages.
// Messages are pre-localized by the server based on Accept-Language.
//==================================================================================================

namespace Fox.TaskFlow.Blazor.Exceptions;

//==================================================================================================
/// <summary>
/// Exception thrown when validation errors occur.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ValidationException"/> class.
/// </remarks>
/// <param name="messages">The localized validation error messages.</param>
//==================================================================================================
public sealed class ValidationException(List<string> messages) : Exception("Validation failed")
{
    //==============================================================================================
    /// <summary>
    /// Gets the localized error messages.
    /// </summary>
    //==============================================================================================
    public List<string> Messages { get; } = messages;
}
