//==================================================================================================
// Test helper for creating mock localized message provider for validator tests.
// Returns English messages for all resource keys.
//==================================================================================================
using Fox.TaskFlow.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Fox.TaskFlow.Tests.Helpers;

//==================================================================================================
/// <summary>
/// Mock message provider for tests.
/// </summary>
//==================================================================================================
public sealed class TestMessageProvider : LocalizedMessageProvider
{
    private static readonly Dictionary<string, string> Translations = new()
    {
        ["TitleRequired"] = "Title is required",
        ["TitleMinLength"] = "Title must be at least {0} characters",
        ["TitleMaxLength"] = "Title cannot exceed {0} characters",
        ["DescriptionRequired"] = "Description is required",
        ["DescriptionMinLength"] = "Description must be at least {0} characters",
        ["DescriptionMaxLength"] = "Description cannot exceed {0} characters",
        ["DueDateFuture"] = "Due date must be in the future",
        ["TaskNotFound"] = "Task with ID {0} not found",
        ["InvalidTargetStatus"] = "Invalid target status"
    };

    public TestMessageProvider() : base(new TestStringLocalizerFactory())
    {
    }

    public override string GetMessage(string key) => Translations.TryGetValue(key, out var value) ? value : key;

    public override string GetFormatMessage(string key, params object[] args) => string.Format(Translations.TryGetValue(key, out var value) ? value : key, args);
}

//==================================================================================================
/// <summary>
/// Mock string localizer for tests.
/// </summary>
//==================================================================================================
public sealed class TestStringLocalizer : IStringLocalizer
{
    private static readonly Dictionary<string, string> Translations = new()
    {
        ["TitleRequired"] = "Title is required",
        ["TitleMinLength"] = "Title must be at least {0} characters",
        ["TitleMaxLength"] = "Title cannot exceed {0} characters",
        ["DescriptionRequired"] = "Description is required",
        ["DescriptionMinLength"] = "Description must be at least {0} characters",
        ["DescriptionMaxLength"] = "Description cannot exceed {0} characters",
        ["DueDateFuture"] = "Due date must be in the future",
        ["TaskNotFound"] = "Task with ID {0} not found",
        ["InvalidTargetStatus"] = "Invalid target status"
    };

    public LocalizedString this[string name] => new(name, Translations.TryGetValue(name, out var value) ? value : name, false);

    public LocalizedString this[string name, params object[] arguments] => new(name, string.Format(Translations.TryGetValue(name, out var value) ? value : name, arguments), false);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => Translations.Select(kvp => new LocalizedString(kvp.Key, kvp.Value, false));
}

//==================================================================================================
/// <summary>
/// Mock string localizer factory for tests.
/// </summary>
//==================================================================================================
public sealed class TestStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource) => new TestStringLocalizer();

    public IStringLocalizer Create(string baseName, string location) => new TestStringLocalizer();
}
