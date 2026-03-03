//==================================================================================================
// Unit tests for ValidationKit validators ensuring business rule enforcement.
// Tests CreateTaskRequestValidator with ValidationKit fluent assertions.
//==================================================================================================
using Fox.TaskFlow.Application.Configuration;
using Fox.TaskFlow.Application.DTOs.Requests;
using Fox.TaskFlow.Application.Validators;
using Microsoft.Extensions.Options;

namespace Fox.TaskFlow.Tests.Application;

//==================================================================================================
/// <summary>
/// Tests for CreateTaskRequestValidator.
/// </summary>
//==================================================================================================
public sealed class CreateTaskRequestValidatorTests
{
    private readonly CreateTaskRequestValidator validator;

    public CreateTaskRequestValidatorTests()
    {
        var config = Options.Create(new ValidationConfiguration());
        validator = new CreateTaskRequestValidator(config);
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_PassForValidRequest
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_PassForValidRequest()
    {
        var request = new CreateTaskRequest
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
        var request = new CreateTaskRequest
        {
            Title = "",
            Description = "Valid description here.",
            DueDate = null
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskRequest.Title));
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForTooShortTitle
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForTooShortTitle()
    {
        var request = new CreateTaskRequest
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
        var request = new CreateTaskRequest
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
        var request = new CreateTaskRequest
        {
            Title = "Valid Title",
            Description = "Short",
            DueDate = null
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskRequest.Description));
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_FailForPastDueDate
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_FailForPastDueDate()
    {
        var request = new CreateTaskRequest
        {
            Title = "Valid Task Title",
            Description = "This is a valid description with enough characters.",
            DueDate = DateTime.UtcNow.AddDays(-1)
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateTaskRequest.DueDate));
    }

    //==============================================================================================
    /// <summary>
    /// Validator_Should_PassForNoDueDate
    /// </summary>
    //==============================================================================================
    [Fact]
    public void Validator_Should_PassForNoDueDate()
    {
        var request = new CreateTaskRequest
        {
            Title = "Valid Task Title",
            Description = "This is a valid description with enough characters.",
            DueDate = null
        };

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
}
