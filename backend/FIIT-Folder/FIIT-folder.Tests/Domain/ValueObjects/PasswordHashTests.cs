using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for PasswordHash value object.
/// </summary>
public class PasswordHashTests
{
    [Fact]
    public void Create_ShouldSucceed_WhenHashIsValid()
    {
        // Arrange
        var hash = "valid_password_hash";

        // Act
        var passwordHash = PasswordHash.Create(hash);

        // Assert
        Assert.NotNull(passwordHash);
        Assert.Equal(hash, passwordHash.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_ShouldThrowException_WhenHashIsNullOrWhitespace(string? hash)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => PasswordHash.Create(hash!));
        Assert.Contains("не может быть пустым", ex.Message);
    }

    [Fact]
    public void Constructor_ShouldAcceptAnyValue()
    {
        // This constructor is for direct instantiation
        // Act
        var passwordHash = new PasswordHash("any_hash");

        // Assert
        Assert.Equal("any_hash", passwordHash.Value);
    }

    [Fact]
    public void ToString_ShouldReturnMaskedValue()
    {
        // Arrange
        var passwordHash = PasswordHash.Create("secret_hash");

        // Act & Assert
        Assert.Equal("***", passwordHash.ToString());
    }
}
