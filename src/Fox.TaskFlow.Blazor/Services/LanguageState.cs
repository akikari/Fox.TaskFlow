//==================================================================================================
// State management for selected language with in-memory persistence.
// Notifies components when language changes.
//==================================================================================================

namespace Fox.TaskFlow.Blazor.Services;

//==================================================================================================
/// <summary>
/// Service for managing the current language selection.
/// </summary>
//==================================================================================================
public sealed class LanguageState
{
    #region Fields

    private string currentLanguage = "en";

    #endregion

    #region Properties

    //==============================================================================================
    /// <summary>
    /// Gets the current language code.
    /// </summary>
    //==============================================================================================
    public string CurrentLanguage => currentLanguage;

    #endregion

    #region Public Methods

    //==============================================================================================
    /// <summary>
    /// Sets the current language and notifies subscribers.
    /// </summary>
    /// <param name="language">The language code (e.g., "en", "hu").</param>
    //==============================================================================================
    public void SetLanguage(string language)
    {
        if (currentLanguage != language)
        {
            currentLanguage = language;
            OnLanguageChanged?.Invoke();
        }
    }

    #endregion

    #region Events

    //==============================================================================================
    /// <summary>
    /// Event raised when the language changes.
    /// </summary>
    //==============================================================================================
    public event Action? OnLanguageChanged;

    #endregion
}
