using Xunit;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.ValueObjects;

/// <summary>
/// Tests for SubjectId value object.
/// </summary>
public class SubjectIdTests
{
    [Fact]
    public void Constructor_ShouldSetValue()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var subjectId = new SubjectId(guid);

        // Assert
        Assert.Equal(guid, subjectId.Value);
    }

    [Fact]
    public void New_ShouldCreateUniqueId()
    {
        // Act
        var subjectId1 = SubjectId.New();
        var subjectId2 = SubjectId.New();

        // Assert
        Assert.NotEqual(subjectId1.Value, subjectId2.Value);
        Assert.NotEqual(Guid.Empty, subjectId1.Value);
        Assert.NotEqual(Guid.Empty, subjectId2.Value);
    }

    [Fact]
    public void ToString_ShouldReturnGuidString()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var subjectId = new SubjectId(guid);

        // Act
        var result = subjectId.ToString();

        // Assert
        Assert.Equal(guid.ToString(), result);
    }

    [Fact]
    public void Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var subjectId1 = new SubjectId(guid);
        var subjectId2 = new SubjectId(guid);
        var subjectId3 = SubjectId.New();

        // Assert
        Assert.Equal(subjectId1, subjectId2);
        Assert.NotEqual(subjectId1, subjectId3);
    }
}
