using Xunit;
using FIIT_folder.Api.Validators;
using FIIT_folder.Api.Models;
using Moq;
using Microsoft.AspNetCore.Http;

namespace FIIT_folder.Tests.Api.Validators;

/// <summary>
/// Tests for UploadMaterialRequestValidator.
/// </summary>
public class UploadMaterialRequestValidatorTests
{
    private readonly UploadMaterialRequestValidator _validator;

    public UploadMaterialRequestValidatorTests()
    {
        _validator = new UploadMaterialRequestValidator();
    }

    [Fact]
    public void Should_HaveNoErrors_WhenRequestIsValid()
    {
        // Arrange
        var fileMock = CreateMockFile("test.pdf", 1024);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            Description = "Test",
            MaterialType = "Exam",
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_HaveError_WhenSubjectIdIsEmpty()
    {
        // Arrange
        var fileMock = CreateMockFile("test.pdf", 1024);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.Empty,
            Year = 2023,
            Semester = 1,
            MaterialType = "Exam",
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "SubjectId");
    }

    [Theory]
    [InlineData(2018)]
    [InlineData(2015)]
    public void Should_HaveError_WhenYearIsTooOld(int year)
    {
        // Arrange
        var fileMock = CreateMockFile("test.pdf", 1024);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = year,
            Semester = 1,
            MaterialType = "Exam",
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Year");
    }

    [Theory]
    [InlineData("")]
    public void Should_HaveError_WhenMaterialTypeIsEmpty(string materialType)
    {
        // Arrange
        var fileMock = CreateMockFile("test.pdf", 1024);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = materialType,
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MaterialType");
    }

    [Theory]
    [InlineData("Unknown")]
    [InlineData("Invalid")]
    public void Should_HaveError_WhenMaterialTypeIsInvalid(string materialType)
    {
        // Arrange
        var fileMock = CreateMockFile("test.pdf", 1024);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = materialType,
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MaterialType");
    }

    [Fact]
    public void Should_HaveError_WhenFileIsNull()
    {
        // Arrange
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = "Exam",
            File = null
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "File");
    }

    [Fact]
    public void Should_HaveError_WhenFileIsEmpty()
    {
        // Arrange
        var fileMock = CreateMockFile("test.pdf", 0);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = "Exam",
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Length"));
    }

    [Fact]
    public void Should_HaveError_WhenFileExceeds25MB()
    {
        // Arrange
        var fileMock = CreateMockFile("test.pdf", 26 * 1024 * 1024); // 26 MB
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = "Exam",
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("Length"));
    }

    [Theory]
    [InlineData("test.exe")]
    [InlineData("test.zip")]
    [InlineData("test.rar")]
    [InlineData("test.mp3")]
    public void Should_HaveError_WhenFileExtensionIsNotAllowed(string fileName)
    {
        // Arrange
        var fileMock = CreateMockFile(fileName, 1024);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = "Exam",
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName.Contains("FileName"));
    }

    [Theory]
    [InlineData("test.pdf")]
    [InlineData("test.doc")]
    [InlineData("test.docx")]
    [InlineData("test.xls")]
    [InlineData("test.xlsx")]
    [InlineData("test.png")]
    [InlineData("test.jpg")]
    [InlineData("test.jpeg")]
    [InlineData("test.ppt")]
    [InlineData("test.pptx")]
    [InlineData("test.txt")]
    public void Should_AcceptValidFileExtensions(string fileName)
    {
        // Arrange
        var fileMock = CreateMockFile(fileName, 1024);
        var request = new UploadMaterialRequest
        {
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = "Exam",
            File = fileMock.Object
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName.Contains("FileName"));
    }

    private static Mock<IFormFile> CreateMockFile(string fileName, long length)
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(length);
        return fileMock;
    }
}
