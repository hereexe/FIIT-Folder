using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for Login value object.
/// </summary>
public class LoginTests
{
    [Theory]
    [InlineData("testuser")]
    [InlineData("user123")]
    [InlineData("test_user")]
    [InlineData("USER")]
    [InlineData("abcd")]
    public void Create_ShouldSucceed_WhenValueIsValid(string value)
    {
        // Act
        var login = Login.Create(value);

        // Assert
        Assert.NotNull(login);
        Assert.Equal(value.ToLowerInvariant(), login.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldThrowException_WhenValueIsNullOrWhitespace(string? value)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Login.Create(value!));
    }

    [Theory]
    [InlineData("abc")] // 3 characters - too short
    [InlineData("ab")]
    [InlineData("a")]
    public void Create_ShouldThrowException_WhenValueIsTooShort(string value)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => Login.Create(value));
        Assert.Contains("between 4 and 20", ex.Message);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenValueIsTooLong()
    {
        // Arrange
        var value = new string('a', 21);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => Login.Create(value));
        Assert.Contains("between 4 and 20", ex.Message);
    }

    [Theory]
    [InlineData("user@name")]
    [InlineData("user name")]
    [InlineData("user-name")]
    [InlineData("user.name!")]
    [InlineData("пользователь")]
    public void Create_ShouldThrowException_WhenValueContainsInvalidCharacters(string value)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => Login.Create(value));
        Assert.Contains("not allowed", ex.Message);
    }

    [Fact]
    public void Create_ShouldTrimAndLowercase()
    {
        // Arrange
        var value = "  TestUser  ";

        // Act
        var login = Login.Create(value);

        // Assert
        Assert.Equal("testuser", login.Value);
    }

    [Fact]
    public void Constructor_ShouldAcceptAnyValue()
    {
        // This constructor is for MongoDB deserialization
        // Act
        var login = new Login("any_value");

        // Assert
        Assert.Equal("any_value", login.Value);
    }
}
