using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for UserId value object.
/// </summary>
public class UserIdTests
{
    [Fact]
    public void Constructor_ShouldSucceed_WhenValueIsValid()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var userId = new UserId(guid);

        // Assert
        Assert.Equal(guid, userId.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenValueIsEmpty()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new UserId(Guid.Empty));
        Assert.Contains("cannot be empty", ex.Message);
    }

    [Fact]
    public void New_ShouldCreateUniqueId()
    {
        // Act
        var userId1 = UserId.New();
        var userId2 = UserId.New();

        // Assert
        Assert.NotEqual(userId1.Value, userId2.Value);
        Assert.NotEqual(Guid.Empty, userId1.Value);
        Assert.NotEqual(Guid.Empty, userId2.Value);
    }

    [Fact]
    public void ToString_ShouldReturnGuidString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var userId = new UserId(guid);

        // Act
        var result = userId.ToString();

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var userId1 = new UserId(guid);
        var userId2 = new UserId(guid);
        var userId3 = UserId.New();

        // Assert
        Assert.Equal(userId1, userId2);
        Assert.NotEqual(userId1, userId3);
    }
}
