using Xunit;
using FIIT_folder.Api.Validators;
using FIIT_folder.Api.Models;

namespace FIIT_folder.Tests.Api.Validators;

/// <summary>
/// Tests for CreateSubjectRequestValidator.
/// </summary>
public class CreateSubjectRequestValidatorTests
{
    private readonly CreateSubjectRequestValidator _validator;

    public CreateSubjectRequestValidatorTests()
    {
        _validator = new CreateSubjectRequestValidator();
    }

    [Fact]
    public void Should_HaveNoErrors_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "Mathematics",
            Semester = 1,
            MaterialTypes = new List<string> { "Exam", "Colloquium" }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_HaveError_WhenNameIsEmpty()
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "",
            Semester = 1,
            MaterialTypes = new List<string> { "Exam" }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_HaveError_WhenNameExceeds50Characters()
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = new string('a', 51),
            Semester = 1,
            MaterialTypes = new List<string> { "Exam" }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name" && e.ErrorMessage.Contains("50"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(9)]
    [InlineData(100)]
    public void Should_HaveError_WhenSemesterIsOutOfRange(int semester)
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "Math",
            Semester = semester,
            MaterialTypes = new List<string> { "Exam" }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Semester");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(8)]
    public void Should_HaveNoError_WhenSemesterIsInRange(int semester)
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "Math",
            Semester = semester,
            MaterialTypes = new List<string> { "Exam" }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "Semester");
    }

    [Fact]
    public void Should_HaveError_WhenMaterialTypesIsEmpty()
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "Math",
            Semester = 1,
            MaterialTypes = new List<string>()
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MaterialTypes");
    }

    [Fact]
    public void Should_HaveError_WhenMaterialTypeIsUnknown()
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "Math",
            Semester = 1,
            MaterialTypes = new List<string> { "Unknown" }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MaterialTypes");
    }

    [Theory]
    [InlineData("Exam")]
    [InlineData("Colloquium")]
    [InlineData("Pass")]
    [InlineData("ControlWork")]
    [InlineData("ComputerPractice")]
    public void Should_AcceptValidMaterialTypes(string materialType)
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "Math",
            Semester = 1,
            MaterialTypes = new List<string> { materialType }
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "MaterialTypes");
    }
}
