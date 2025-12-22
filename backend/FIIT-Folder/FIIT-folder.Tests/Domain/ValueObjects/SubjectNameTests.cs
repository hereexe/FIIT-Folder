using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for SubjectName value object.
/// </summary>
public class SubjectNameTests
{
    [Theory]
    [InlineData("Mathematics")]
    [InlineData("Physics")]
    [InlineData("Программирование")]
    [InlineData("a")]
    public void Constructor_ShouldSucceed_WhenValueIsValid(string value)
    {
        // Act
        var name = new SubjectName(value);

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
        var ex = Assert.Throws<ArgumentException>(() => new SubjectName(value!));
        Assert.Contains("cannot be null or whitespace", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldTrimWhitespace()
    {
        // Arrange
        var value = "  Math  ";

        // Act
        var name = new SubjectName(value);

        // Assert
        Assert.Equal("Math", name.Value);
    }
}
