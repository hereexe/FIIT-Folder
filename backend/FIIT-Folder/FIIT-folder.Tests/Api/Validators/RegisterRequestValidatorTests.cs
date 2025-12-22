using Xunit;
using FIIT_folder.Api.Validators;
using FIIT_folder.Api.Models;

namespace FIIT_folder.Tests.Api.Validators;

/// <summary>
/// Tests for RegisterRequestValidator.
/// </summary>
public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator;

    public RegisterRequestValidatorTests()
    {
        _validator = new RegisterRequestValidator();
    }

    [Fact]
    public void Should_HaveNoErrors_WhenRequestIsValid()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_HaveError_WhenUsernameIsEmpty(string? username)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = username!,
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Theory]
    [InlineData("ab")] // too short (< 3)
    public void Should_HaveError_WhenUsernameIsTooShort(string username)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = username,
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Fact]
    public void Should_HaveError_WhenUsernameIsTooLong()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = new string('a', 21), // > 20
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Theory]
    [InlineData("user@name")]
    [InlineData("user name")]
    [InlineData("user-name")]
    [InlineData("user!name")]
    public void Should_HaveError_WhenUsernameContainsInvalidCharacters(string username)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = username,
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Theory]
    [InlineData("user123")]
    [InlineData("user_name")]
    [InlineData("user.name")]
    [InlineData("USERNAME")]
    public void Should_AcceptValidUsernames(string username)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = username,
            Password = "password123"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "Username");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_HaveError_WhenPasswordIsEmpty(string? password)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Password = password!
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Theory]
    [InlineData("12345")] // 5 characters
    [InlineData("abc")]
    public void Should_HaveError_WhenPasswordIsTooShort(string password)
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Password = password
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public void Should_AcceptValidPassword()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Password = "123456" // exactly 6 characters
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        Assert.DoesNotContain(result.Errors, e => e.PropertyName == "Password");
    }
}
