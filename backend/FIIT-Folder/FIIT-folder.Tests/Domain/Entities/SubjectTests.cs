using Xunit;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Enums;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Domain.Entities;

/// <summary>
/// Tests for Subject Domain Entity logic.
/// </summary>
public class SubjectTests
{
    [Fact]
    public void Create_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = SubjectId.New();
        var name = new SubjectName("Math");
        var semester = new SubjectSemester(1);
        var types = new List<MaterialType> { MaterialType.Exam };

        // Act
        var subject = new Subject(id, name, semester, types);

        // Assert
        Assert.Equal(id, subject.Id);
        Assert.Equal(name, subject.Name);
        Assert.Equal(semester, subject.Semester);
        Assert.Single(subject.AvailableMaterialTypes);
        Assert.Equal(MaterialType.Exam, subject.AvailableMaterialTypes.First());
    }

    [Fact]
    public void AddMaterialType_ShouldAddType_WhenNotExists()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam });

        // Act
        subject.AddMaterialType(MaterialType.Colloquium);

        // Assert
        Assert.Equal(2, subject.AvailableMaterialTypes.Count);
        Assert.Contains(MaterialType.Colloquium, subject.AvailableMaterialTypes);
    }

    [Fact]
    public void AddMaterialType_ShouldNotAddType_WhenAlreadyExists()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam });

        // Act
        subject.AddMaterialType(MaterialType.Exam);

        // Assert
        Assert.Single(subject.AvailableMaterialTypes);
    }

    [Fact]
    public void RemoveMaterialType_ShouldRemoveType_WhenMultipleTypesExist()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam, MaterialType.Colloquium });

        // Act
        subject.RemoveMaterialType(MaterialType.Exam);

        // Assert
        Assert.Single(subject.AvailableMaterialTypes);
        Assert.Contains(MaterialType.Colloquium, subject.AvailableMaterialTypes);
    }

    [Fact]
    public void RemoveMaterialType_ShouldThrowException_WhenOnlyOneTypeExists()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam });

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => subject.RemoveMaterialType(MaterialType.Exam));
        Assert.Equal("Нельзя удалить последний тип материала", ex.Message);
    }

    [Fact]
    public void UpdateName_ShouldChangeSubjectName()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam });

        // Act
        subject.UpdateName("Physics");

        // Assert
        Assert.Equal("Physics", subject.Name.Value);
    }

    [Fact]
    public void SetMaterialTypes_ShouldReplaceAllTypes()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam });

        // Act
        subject.SetMaterialTypes(new[] { MaterialType.Pass, MaterialType.ControlWork });

        // Assert
        Assert.Equal(2, subject.AvailableMaterialTypes.Count);
        Assert.Contains(MaterialType.Pass, subject.AvailableMaterialTypes);
        Assert.Contains(MaterialType.ControlWork, subject.AvailableMaterialTypes);
    }

    [Fact]
    public void SetMaterialTypes_ShouldThrowException_WhenEmptyList()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam });

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => subject.SetMaterialTypes(new List<MaterialType>()));
        Assert.Equal("Предмет должен иметь хотя бы один тип материала", ex.Message);
    }

    [Fact]
    public void HasMaterialType_ShouldReturnTrue_WhenTypeExists()
    {
        // Arrange
        var subject = new Subject(SubjectId.New(), new SubjectName("Math"), new SubjectSemester(1), new[] { MaterialType.Exam });

        // Act & Assert
        Assert.True(subject.HasMaterialType(MaterialType.Exam));
        Assert.False(subject.HasMaterialType(MaterialType.Pass));
    }
}
