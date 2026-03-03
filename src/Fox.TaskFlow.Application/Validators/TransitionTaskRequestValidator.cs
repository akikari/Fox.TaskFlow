//==================================================================================================
// Validator for TransitionTaskRequest using Fox.ValidationKit.
// Ensures the target status is a valid enum value.
//==================================================================================================
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.ValidationKit;

namespace Fox.TaskFlow.Application.Validators;

//==================================================================================================
/// <summary>
/// Validates the <see cref="TransitionTaskRequest"/> model.
/// </summary>
//==================================================================================================
public sealed class TransitionTaskRequestValidator : Validator<TransitionTaskRequest>
{
    //==============================================================================================
    /// <summary>
    /// Initializes a new instance of the <see cref="TransitionTaskRequestValidator"/> class.
    /// </summary>
    /// <param name="localizerFactory">The string localizer factory.</param>
    //==============================================================================================
    public TransitionTaskRequestValidator()
    {
        RuleFor(x => x.TargetStatus)
            .IsInEnum(ValidationMessageKeys.InvalidTargetStatus);
    }
}
