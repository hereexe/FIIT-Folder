using Xunit;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Domain.Entities;

/// <summary>
/// Tests for StudyMaterial entity.
/// </summary>
public class StudyMaterialTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var name = new MaterialName("Lecture.pdf");
        var subjectId = SubjectId.New();
        var userId = UserId.New();
        var year = new StudyYear(2023);
        var semester = new Semester(1);
        var description = "Test description";
        var size = new MaterialSize(1024);
        var materialType = MaterialType.Exam;
        var filePath = new ResourceLocation("path/to/file.pdf");

        // Act
        var material = new StudyMaterial(name, subjectId, userId, year, semester, description, size, materialType, filePath);

        // Assert
        Assert.NotNull(material.Id);
        Assert.NotEqual(Guid.Empty, material.Id.Value);
        Assert.Equal(name, material.Name);
        Assert.Equal(subjectId, material.SubjectId);
        Assert.Equal(userId, material.UserId);
        Assert.Equal(year, material.Year);
        Assert.Equal(semester, material.Semester);
        Assert.Equal(description, material.Description);
        Assert.Equal(size, material.Size);
        Assert.Equal(materialType, material.MaterialType);
        Assert.Equal(filePath, material.FilePath);
    }

    [Fact]
    public void Constructor_ShouldSetUploadedAtToCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;
        
        var material = new StudyMaterial(
            new MaterialName("Test.pdf"),
            SubjectId.New(),
            UserId.New(),
            new StudyYear(2023),
            new Semester(1),
            "Description",
            new MaterialSize(100),
            MaterialType.Exam,
            new ResourceLocation("path/to/file.pdf"));

        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(material.UploadedAt >= beforeCreation && material.UploadedAt <= afterCreation);
    }

    [Fact]
    public void Constructor_ShouldGenerateUniqueIds()
    {
        // Arrange & Act
        var material1 = new StudyMaterial(
            new MaterialName("Test1.pdf"),
            SubjectId.New(),
            UserId.New(),
            new StudyYear(2023),
            new Semester(1),
            "Description",
            new MaterialSize(100),
            MaterialType.Exam,
            new ResourceLocation("path/to/file1.pdf"));

        var material2 = new StudyMaterial(
            new MaterialName("Test2.pdf"),
            SubjectId.New(),
            UserId.New(),
            new StudyYear(2023),
            new Semester(1),
            "Description",
            new MaterialSize(100),
            MaterialType.Exam,
            new ResourceLocation("path/to/file2.pdf"));

        // Assert
        Assert.NotEqual(material1.Id.Value, material2.Id.Value);
    }

    [Theory]
    [InlineData(MaterialType.Exam)]
    [InlineData(MaterialType.Colloquium)]
    [InlineData(MaterialType.Pass)]
    [InlineData(MaterialType.ControlWork)]
    [InlineData(MaterialType.ComputerPractice)]
    public void Constructor_ShouldAcceptAllMaterialTypes(MaterialType materialType)
    {
        // Arrange & Act
        var material = new StudyMaterial(
            new MaterialName("Test.pdf"),
            SubjectId.New(),
            UserId.New(),
            new StudyYear(2023),
            new Semester(1),
            "Description",
            new MaterialSize(100),
            materialType,
            new ResourceLocation("path/to/file.pdf"));

        // Assert
        Assert.Equal(materialType, material.MaterialType);
    }
}
