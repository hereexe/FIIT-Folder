using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for StudyMaterialId value object.
/// </summary>
public class StudyMaterialIdTests
{
    [Fact]
    public void Constructor_ShouldSetValue()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var materialId = new StudyMaterialId(guid);

        // Assert
        Assert.Equal(guid, materialId.Value);
    }

    [Fact]
    public void New_ShouldCreateUniqueId()
    {
        // Act
        var materialId1 = StudyMaterialId.New();
        var materialId2 = StudyMaterialId.New();

        // Assert
        Assert.NotEqual(materialId1.Value, materialId2.Value);
        Assert.NotEqual(Guid.Empty, materialId1.Value);
        Assert.NotEqual(Guid.Empty, materialId2.Value);
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var materialId1 = new StudyMaterialId(guid);
        var materialId2 = new StudyMaterialId(guid);
        var materialId3 = StudyMaterialId.New();

        // Assert
        Assert.Equal(materialId1, materialId2);
        Assert.NotEqual(materialId1, materialId3);
    }
}
