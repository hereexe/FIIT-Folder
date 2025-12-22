using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for SubjectSemester value object.
/// </summary>
public class SubjectSemesterTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Constructor_ShouldSucceed_WhenValueIsInValidRange(int value)
    {
        // Act
        var semester = new SubjectSemester(value);

        // Assert
        Assert.Equal(value, semester.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Constructor_ShouldThrowException_WhenValueIsLessThan1(int value)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new SubjectSemester(value));
        Assert.Contains("between 1 and 8", ex.Message);
    }

    [Theory]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(100)]
    public void Constructor_ShouldThrowException_WhenValueIsGreaterThan8(int value)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new SubjectSemester(value));
        Assert.Contains("between 1 and 8", ex.Message);
    }
}
