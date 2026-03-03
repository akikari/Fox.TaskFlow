//==================================================================================================
// Localized validation message provider for lazy evaluation.
// Retrieves localized messages at validation time based on current culture.
//==================================================================================================
using Microsoft.Extensions.Localization;

namespace Fox.TaskFlow.Application.Validators;

//==================================================================================================
/// <summary>
/// Provides localized validation messages that are evaluated lazily.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LocalizedMessageProvider"/> class.
/// </remarks>
/// <param name="localizerFactory">The string localizer factory.</param>
//==================================================================================================
public class LocalizedMessageProvider(IStringLocalizerFactory localizerFactory)
{
    //==============================================================================================
    /// <summary>
    /// Gets a localized message for the specified key.
    /// </summary>
    /// <param name="key">The message key.</param>
    /// <returns>The localized message.</returns>
    //==============================================================================================
    public virtual string GetMessage(string key)
    {
        var localizer = localizerFactory.Create("Resources.ValidationMessages", "Fox.TaskFlow.Application");
        var message = localizer[key];

        return message.ResourceNotFound ? $"[{key}]" : message.Value;
    }

    //==============================================================================================
    /// <summary>
    /// Gets a localized message with format arguments.
    /// </summary>
    /// <param name="key">The message key.</param>
    /// <param name="args">The format arguments.</param>
    /// <returns>The formatted localized message.</returns>
    //==============================================================================================
    public virtual string GetFormatMessage(string key, params object[] args)
    {
        var localizer = localizerFactory.Create("Resources.ValidationMessages", "Fox.TaskFlow.Application");
        var message = localizer[key];

        return message.ResourceNotFound ? $"[{key}]" : string.Format(message.Value, args);
    }
}
