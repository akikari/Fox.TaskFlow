//==================================================================================================
// Unit tests for UpdateTaskRequestValidator ensuring business rule enforcement.
// Tests UpdateTaskRequestValidator with ValidationKit fluent assertions.
//==================================================================================================
using Fox.TaskFlow.Application.Configuration;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.Validators;
using Microsoft.Extensions.Options;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for UpdateTaskRequestValidator.
/// </summary>
//==================================================================================================
public sealed class UpdateTaskRequestValidatorTests
{
    private readonly UpdateTaskRequestValidator validator;

    public UpdateTaskRequestValidatorTests()
    {
        var config = Options.Create(new ValidationConfiguration());
        validator = new UpdateTaskRequestValidator(config);
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_PassForValidRequest
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_PassForValidRequest()
    {
        var request = new UpdateTaskRequest
        {
            Title = "Valid Task Title",
            Description = "This is a valid description with enough characters.",
            DueDate = DateTime.UtcNow.AddDays(7)
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForEmptyTitle
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForEmptyTitle()
    {
        var request = new UpdateTaskRequest
        {
            Title = "",
            Description = "Valid description here.",
            DueDate = null
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskRequest.Title));
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForTooShortTitle
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForTooShortTitle()
    {
        var request = new UpdateTaskRequest
        {
            Title = "AB",
            Description = "Valid description here.",
            DueDate = null
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForTooLongTitle
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForTooLongTitle()
    {
        var request = new UpdateTaskRequest
        {
            Title = new string('A', 201),
            Description = "Valid description here.",
            DueDate = null
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForTooShortDescription
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForTooShortDescription()
    {
        var request = new UpdateTaskRequest
        {
            Title = "Valid Title",
            Description = "Short",
            DueDate = null
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskRequest.Description));
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForPastDueDate
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForPastDueDate()
    {
        var request = new UpdateTaskRequest
        {
            Title = "Valid Task Title",
            Description = "This is a valid description with enough characters.",
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(UpdateTaskRequest.DueDate));
    }
}
