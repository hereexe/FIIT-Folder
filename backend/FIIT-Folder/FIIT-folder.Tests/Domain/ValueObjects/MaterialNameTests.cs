using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for MaterialName value object.
/// </summary>
public class MaterialNameTests
{
    [Theory]
    [InlineData("Lecture.pdf")]
    [InlineData("Exam 2023")]
    [InlineData("a")]
    public void Constructor_ShouldSucceed_WhenValueIsValid(string value)
    {
        // Act
        var name = new MaterialName(value);

        // Assert
        Assert.Equal(value.Trim(), name.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowException_WhenValueIsNullOrWhitespace(string? value)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new MaterialName(value!));
        Assert.Contains("cannot be null or whitespace", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenValueExceedsMaxLength()
    {
        // Arrange
        var value = new string('a', 201); // MaxLength is 200

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new MaterialName(value));
        Assert.Contains("cannot be longer than 200", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldAcceptMaxLengthValue()
    {
        // Arrange
        var value = new string('a', 200);

        // Act
        var name = new MaterialName(value);

        // Assert
        Assert.Equal(200, name.Value.Length);
    }

    [Fact]
    public void Constructor_ShouldTrimWhitespace()
    {
        // Arrange
        var value = "  Test Material  ";

        // Act
        var name = new MaterialName(value);

        // Assert
        Assert.Equal("Test Material", name.Value);
    }

    [Fact]
    public void ToString_ShouldReturnValue()
    {
        // Arrange
        var name = new MaterialName("Test.pdf");

        // Act & Assert
        Assert.Equal("Test.pdf", name.ToString());
    }
}
