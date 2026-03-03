//==================================================================================================
// Unit tests for TransitionTaskRequestValidator ensuring enum validation.
// Tests TransitionTaskRequestValidator with ValidationKit fluent assertions.
//==================================================================================================
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.Validators;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for TransitionTaskRequestValidator.
/// </summary>
//==================================================================================================
public sealed class TransitionTaskRequestValidatorTests
{
    private readonly TransitionTaskRequestValidator validator;

    public TransitionTaskRequestValidatorTests()
    {
        validator = new TransitionTaskRequestValidator();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_PassForValidCreatedStatus
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_PassForValidCreatedStatus()
    {
        var request = new TransitionTaskRequest { TargetStatus = TaskStatus.Created };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_PassForValidInProgressStatus
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_PassForValidInProgressStatus()
    {
        var request = new TransitionTaskRequest { TargetStatus = TaskStatus.InProgress };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_PassForValidCompletedStatus
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_PassForValidCompletedStatus()
    {
        var request = new TransitionTaskRequest { TargetStatus = TaskStatus.Completed };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_PassForValidCancelledStatus
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_PassForValidCancelledStatus()
    {
        var request = new TransitionTaskRequest { TargetStatus = TaskStatus.Cancelled };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForInvalidStatus
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForInvalidStatus()
    {
        var request = new TransitionTaskRequest { TargetStatus = (TaskStatus)999 };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(TransitionTaskRequest.TargetStatus));
    }
}
