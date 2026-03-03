//==================================================================================================
// Configuration for validation rules and constraints.
// Centralizes magic numbers for title and description validation.
//==================================================================================================

namespace Fox.TaskFlow.Application.Configuration;

//==================================================================================================
/// <summary>
/// Configuration for validation rules.
/// </summary>
//==================================================================================================
public sealed class ValidationConfiguration
{
    //==============================================================================================
    /// <summary>
    /// Gets or sets the minimum length for task titles.
    /// </summary>
    //==============================================================================================
    public int TitleMinLength { get; set; } = 3;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the maximum length for task titles.
    /// </summary>
    //==============================================================================================
    public int TitleMaxLength { get; set; } = 200;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the minimum length for task descriptions.
    /// </summary>
    //==============================================================================================
    public int DescriptionMinLength { get; set; } = 10;

    //==============================================================================================
    /// <summary>
    /// Gets or sets the maximum length for task descriptions.
    /// </summary>
    //==============================================================================================
    public int DescriptionMaxLength { get; set; } = 2000;
}
