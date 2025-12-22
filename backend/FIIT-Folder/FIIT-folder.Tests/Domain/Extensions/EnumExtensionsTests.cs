using Xunit;
using FIIT_folder.Domain.Extensions;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Domain.Extensions;

/// <summary>
/// Tests for EnumExtensions.
/// </summary>
public class EnumExtensionsTests
{
    [Theory]
    [InlineData(MaterialType.Exam, "Экзамены")]
    [InlineData(MaterialType.Colloquium, "Коллоквиумы")]
    [InlineData(MaterialType.Pass, "Зачёты")]
    [InlineData(MaterialType.ControlWork, "Контрольные работы")]
    [InlineData(MaterialType.ComputerPractice, "Компьютерные практикумы")]
    public void GetDescription_ShouldReturnDescription_WhenAttributeExists(MaterialType type, string expected)
    {
        // Act
        var description = type.GetDescription();

        // Assert
        Assert.Equal(expected, description);
    }

    [Theory]
    [InlineData(UserRole.Student)]
    [InlineData(UserRole.Teacher)]
    [InlineData(UserRole.Admin)]
    public void GetDescription_ShouldReturnEnumName_WhenNoAttribute(UserRole role)
    {
        // UserRole enum values don't have Description attributes
        // Act
        var description = role.GetDescription();

        // Assert
        Assert.Equal(role.ToString(), description);
    }

    [Theory]
    [InlineData(RatingType.Like)]
    [InlineData(RatingType.Dislike)]
    public void GetDescription_ShouldReturnEnumName_ForRatingType(RatingType rating)
    {
        // RatingType enum values don't have Description attributes
        // Act
        var description = rating.GetDescription();

        // Assert
        Assert.Equal(rating.ToString(), description);
    }
}
