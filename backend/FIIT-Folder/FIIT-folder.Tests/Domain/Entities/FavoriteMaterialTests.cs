using Xunit;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.Entities;

/// <summary>
/// Tests for FavoriteMaterial entity.
/// </summary>
public class FavoriteMaterialTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var userId = UserId.New();
        var materialId = StudyMaterialId.New();

        // Act
        var favorite = new FavoriteMaterial(userId, materialId);

        // Assert
        Assert.NotEqual(Guid.Empty, favorite.Id);
        Assert.Equal(userId, favorite.UserId);
        Assert.Equal(materialId, favorite.MaterialId);
    }

    [Fact]
    public void Constructor_ShouldSetAddedAtToCurrentTime()
    {
        // Arrange
        var userId = UserId.New();
        var materialId = StudyMaterialId.New();
        var beforeCreation = DateTime.UtcNow;

        // Act
        var favorite = new FavoriteMaterial(userId, materialId);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(favorite.AddedAt >= beforeCreation && favorite.AddedAt <= afterCreation);
    }

    [Fact]
    public void Constructor_ShouldGenerateUniqueIds()
    {
        // Arrange
        var userId = UserId.New();
        var materialId = StudyMaterialId.New();

        // Act
        var favorite1 = new FavoriteMaterial(userId, materialId);
        var favorite2 = new FavoriteMaterial(userId, materialId);

        // Assert
        Assert.NotEqual(favorite1.Id, favorite2.Id);
    }
}
