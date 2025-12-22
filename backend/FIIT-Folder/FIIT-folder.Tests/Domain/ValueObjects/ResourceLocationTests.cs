using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for ResourceLocation value object.
/// </summary>
public class ResourceLocationTests
{
    [Theory]
    [InlineData("file.pdf")]
    [InlineData("path/to/file.pdf")]
    [InlineData("document.doc")]
    [InlineData("document.docx")]
    [InlineData("spreadsheet.xls")]
    [InlineData("spreadsheet.xlsx")]
    [InlineData("image.png")]
    [InlineData("image.jpg")]
    [InlineData("image.jpeg")]
    [InlineData("presentation.ppt")]
    [InlineData("presentation.pptx")]
    [InlineData("text.txt")]
    public void Constructor_ShouldSucceed_WhenExtensionIsAllowed(string path)
    {
        // Act
        var location = new ResourceLocation(path);

        // Assert
        Assert.Equal(path, location.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowException_WhenPathIsEmpty(string? path)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new ResourceLocation(path!));
        Assert.Contains("cannot be empty", ex.Message);
    }

    [Theory]
    [InlineData("file.exe")]
    [InlineData("file.zip")]
    [InlineData("file.rar")]
    [InlineData("file.mp3")]
    [InlineData("file.mp4")]
    [InlineData("file")]
    [InlineData("file.unknown")]
    public void Constructor_ShouldThrowException_WhenExtensionIsNotAllowed(string path)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new ResourceLocation(path));
        Assert.Contains("You can only", ex.Message);
    }

    [Theory]
    [InlineData("file.pdf", true)]
    [InlineData("file.PDF", true)]
    [InlineData("file.Pdf", true)]
    [InlineData("file.doc", true)]
    [InlineData("file.exe", false)]
    [InlineData("file", false)]
    public void IsAllowedExtension_ShouldReturnCorrectValue(string fileName, bool expected)
    {
        // Act
        var result = ResourceLocation.IsAllowedExtension(fileName);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Constructor_ShouldBeCaseInsensitive_ForExtensions()
    {
        // Act & Assert - should not throw
        var location1 = new ResourceLocation("file.PDF");
        var location2 = new ResourceLocation("file.Pdf");
        var location3 = new ResourceLocation("file.pDf");

        Assert.NotNull(location1);
        Assert.NotNull(location2);
        Assert.NotNull(location3);
    }
}
