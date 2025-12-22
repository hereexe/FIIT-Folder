using Xunit;
using FIIT_folder.Api.Validators;
using FIIT_folder.Api.Models;

namespace FIIT_folder.Tests.Api.Validators;

/// <summary>
/// Tests for GetMaterialsRequestValidator.
/// </summary>
public class GetMaterialsRequestValidatorTests
{
    private readonly GetMaterialsRequestValidator _validator;

    public GetMaterialsRequestValidatorTests()
    {
        _validator = new GetMaterialsRequestValidator();
    }

    [Fact]
    public void Should_HaveNoErrors_WhenRequestIsValid()
    {
        // Arrange
        var request = new GetMaterialsRequest
        {
            SubjectId = Guid.NewGuid(),
            MaterialType = "Exam"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_HaveNoErrors_WhenMaterialTypeIsNull()
    {
        // Arrange
        var request = new GetMaterialsRequest
        {
            SubjectId = Guid.NewGuid(),
            MaterialType = null
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_HaveNoErrors_WhenMaterialTypeIsEmpty()
    {
        // Arrange
        var request = new GetMaterialsRequest
        {
            SubjectId = Guid.NewGuid(),
            MaterialType = ""
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
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
        var request = new GetMaterialsRequest
        {
            SubjectId = Guid.NewGuid(),
            MaterialType = materialType
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "MaterialType");
    }

    [Theory]
    [InlineData("Unknown")]
    [InlineData("Invalid")]
    [InlineData("Lecture")]
    public void Should_HaveError_WhenMaterialTypeIsInvalid(string materialType)
    {
        // Arrange
        var request = new GetMaterialsRequest
        {
            SubjectId = Guid.NewGuid(),
            MaterialType = materialType
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MaterialType");
    }

    [Fact]
    public void Should_HaveNoErrors_WhenSubjectIdIsNull()
    {
        // SubjectId is optional for global search
        // Arrange
        var request = new GetMaterialsRequest
        {
            SubjectId = null,
            MaterialType = "Exam"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }
}
