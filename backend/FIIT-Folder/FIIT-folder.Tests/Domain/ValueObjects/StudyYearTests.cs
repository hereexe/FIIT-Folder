using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for StudyYear value object.
/// </summary>
public class StudyYearTests
{
    [Theory]
    [InlineData(2018)]
    [InlineData(2019)]
    [InlineData(2020)]
    [InlineData(2023)]
    [InlineData(2024)]
    public void Constructor_ShouldSucceed_WhenValueIsValid(int value)
    {
        // Act
        var year = new StudyYear(value);

        // Assert
        Assert.Equal(value, year.Value);
    }

    [Fact]
    public void Constructor_ShouldSucceed_ForCurrentYear()
    {
        // Arrange
        var currentYear = DateTime.UtcNow.Year;

        // Act
        var year = new StudyYear(currentYear);

        // Assert
        Assert.Equal(currentYear, year.Value);
    }

    [Fact]
    public void Constructor_ShouldSucceed_ForNextTwoYears()
    {
        // Arrange
        var nextYear = DateTime.UtcNow.Year + 1;
        var yearAfterNext = DateTime.UtcNow.Year + 2;

        // Act & Assert - should not throw
        var year1 = new StudyYear(nextYear);
        var year2 = new StudyYear(yearAfterNext);

        Assert.Equal(nextYear, year1.Value);
        Assert.Equal(yearAfterNext, year2.Value);
    }

    [Theory]
    [InlineData(2017)]
    [InlineData(2016)]
    [InlineData(2000)]
    public void Constructor_ShouldThrowException_WhenValueIsBefore2018(int value)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new StudyYear(value));
        Assert.Contains("2018", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenValueIsMoreThan2YearsInFuture()
    {
        // Arrange
        var futureYear = DateTime.UtcNow.Year + 3;

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new StudyYear(futureYear));
        Assert.Contains("2 years into the future", ex.Message);
    }
}
