using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for MaterialSize value object.
/// </summary>
public class MaterialSizeTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(1024)]
    [InlineData(1024 * 1024)] // 1 MB
    [InlineData(10 * 1024 * 1024)] // 10 MB - max allowed
    public void Constructor_ShouldSucceed_WhenSizeIsValid(long size)
    {
        // Act
        var materialSize = new MaterialSize(size);

        // Assert
        Assert.Equal(size, materialSize.Size);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_ShouldThrowException_WhenSizeIsNegative(long size)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new MaterialSize(size));
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenSizeExceeds10MB()
    {
        // Arrange
        var size = 10 * 1024 * 1024 + 1; // 10 MB + 1 byte

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new MaterialSize(size));
    }

    [Fact]
    public void ToString_ShouldReturnBytes_WhenLessThan1KB()
    {
        // Arrange
        var materialSize = new MaterialSize(500);

        // Act
        var result = materialSize.ToString();

        // Assert
        Assert.Equal("500 B", result);
    }

    [Fact]
    public void ToString_ShouldReturnKilobytes_WhenBetween1KBAnd1MB()
    {
        // Arrange
        var materialSize = new MaterialSize(2048); // 2 KB

        // Act
        var result = materialSize.ToString();

        // Assert
        Assert.Contains("KB", result);
        Assert.StartsWith("2", result);
    }

    [Fact]
    public void ToString_ShouldReturnMegabytes_WhenGreaterThan1MB()
    {
        // Arrange
        var materialSize = new MaterialSize(2 * 1024 * 1024); // 2 MB

        // Act
        var result = materialSize.ToString();

        // Assert
        Assert.Contains("MB", result);
        Assert.StartsWith("2", result);
    }

    [Fact]
    public void ToString_ShouldFormatWithTwoDecimalPlaces()
    {
        // Arrange
        var materialSize = new MaterialSize(1536); // 1.5 KB

        // Act
        var result = materialSize.ToString();

        // Assert - use Contains instead of exact match due to locale differences
        Assert.Contains("1", result);
        Assert.Contains("50", result);
        Assert.Contains("KB", result);
    }
}
