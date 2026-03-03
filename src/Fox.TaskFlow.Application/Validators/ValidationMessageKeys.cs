//==================================================================================================
// Central constants for validation message keys using nameof for type safety.
// Prevents string literal duplication and enables compile-time checking.
//==================================================================================================

namespace Fox.TaskFlow.Application.Validators;

//==================================================================================================
/// <summary>
/// Central constants for validation message keys.
/// </summary>
//==================================================================================================
public static class ValidationMessageKeys
{
    //==============================================================================================
    /// <summary>
    /// Message key for required title validation.
    /// </summary>
    //==============================================================================================
    public const string TitleRequired = nameof(TitleRequired);

    //==============================================================================================
    /// <summary>
    /// Message key for title minimum length validation.
    /// </summary>
    //==============================================================================================
    public const string TitleMinLength = nameof(TitleMinLength);

    //==============================================================================================
    /// <summary>
    /// Message key for title maximum length validation.
    /// </summary>
    //==============================================================================================
    public const string TitleMaxLength = nameof(TitleMaxLength);

    //==============================================================================================
    /// <summary>
    /// Message key for required description validation.
    /// </summary>
    //==============================================================================================
    public const string DescriptionRequired = nameof(DescriptionRequired);

    //==============================================================================================
    /// <summary>
    /// Message key for description minimum length validation.
    /// </summary>
    //==============================================================================================
    public const string DescriptionMinLength = nameof(DescriptionMinLength);

    //==============================================================================================
    /// <summary>
    /// Message key for description maximum length validation.
    /// </summary>
    //==============================================================================================
    public const string DescriptionMaxLength = nameof(DescriptionMaxLength);

    //==============================================================================================
    /// <summary>
    /// Message key for future due date validation.
    /// </summary>
    //==============================================================================================
    public const string DueDateFuture = nameof(DueDateFuture);

    //==============================================================================================
    /// <summary>
    /// Message key for invalid target status validation.
    /// </summary>
    //==============================================================================================
    public const string InvalidTargetStatus = nameof(InvalidTargetStatus);

    //==============================================================================================
    /// <summary>
    /// Message key for task not found error.
    /// </summary>
    //==============================================================================================
    public const string TaskNotFound = nameof(TaskNotFound);
}
