//==================================================================================================
// Validator for CreateTaskRequest using Fox.ValidationKit with localization.
// Ensures title and description meet business rules with fail-fast per property.
//==================================================================================================
using Fox.TaskFlow.Application.Configuration;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.ValidationKit;
using Microsoft.Extensions.Options;

namespace Fox.TaskFlow.Application.Validators;

//==================================================================================================
/// <summary>
/// Validates the <see cref="CreateTaskRequest"/> model.
/// </summary>
//==================================================================================================
public sealed class CreateTaskRequestValidator : Validator<CreateTaskRequest>
{
    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTaskRequestValidator"/> class.
    /// </summary>
    /// <param name="validationConfig">The validation configuration.</param>
    //==============================================================================================
    public CreateTaskRequestValidator(IOptions<ValidationConfiguration> validationConfig)
    {
        ArgumentNullException.ThrowIfNull(validationConfig);

        var config = validationConfig.Value;

        RuleFor(x => x.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty(ValidationMessageKeys.TitleRequired)
            .MinLength(config.TitleMinLength, ValidationMessageKeys.TitleMinLength)
            .MaxLength(config.TitleMaxLength, ValidationMessageKeys.TitleMaxLength);

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty(ValidationMessageKeys.DescriptionRequired)
            .MinLength(config.DescriptionMinLength, ValidationMessageKeys.DescriptionMinLength)
            .MaxLength(config.DescriptionMaxLength, ValidationMessageKeys.DescriptionMaxLength);

        RuleFor(x => x.DueDate)
            .Custom((request, dueDate) => !dueDate.HasValue || dueDate.Value > DateTime.UtcNow, ValidationMessageKeys.DueDateFuture);
    }
}
