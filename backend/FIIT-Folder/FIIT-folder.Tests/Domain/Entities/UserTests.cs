using Xunit;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Domain.Entities;

/// <summary>
/// Tests for User entity.
/// </summary>
public class UserTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var userId = UserId.New();
        var login = Login.Create("testuser");
        var passwordHash = PasswordHash.Create("hashed_password");
        var role = UserRole.Student;

        // Act
        var user = new User(userId, login, passwordHash, role);

        // Assert
        Assert.Equal(userId, user.Id);
        Assert.Equal(login, user.Login);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.Equal(role, user.Role);
    }

    [Fact]
    public void Constructor_ShouldSetCreatedAtToCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;
        
        var user = new User(
            UserId.New(),
            Login.Create("testuser"),
            PasswordHash.Create("hash"),
            UserRole.Student);

        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(user.CreatedAt >= beforeCreation && user.CreatedAt <= afterCreation);
    }

    [Fact]
    public void UpdatePassword_ShouldChangePasswordHash()
    {
        // Arrange
        var user = new User(
            UserId.New(),
            Login.Create("testuser"),
            PasswordHash.Create("old_hash"),
            UserRole.Student);

        var newPasswordHash = PasswordHash.Create("new_hash");

        // Act
        user.UpdatePassword(newPasswordHash);

        // Assert
        Assert.Equal(newPasswordHash, user.PasswordHash);
    }

    [Theory]
    [InlineData(UserRole.Student)]
    [InlineData(UserRole.Teacher)]
    [InlineData(UserRole.Admin)]
    public void Constructor_ShouldAcceptAllUserRoles(UserRole role)
    {
        // Arrange & Act
        var user = new User(
            UserId.New(),
            Login.Create("testuser"),
            PasswordHash.Create("hash"),
            role);

        // Assert
        Assert.Equal(role, user.Role);
    }
}
