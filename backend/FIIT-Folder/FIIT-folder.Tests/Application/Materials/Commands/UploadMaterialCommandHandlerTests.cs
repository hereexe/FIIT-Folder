using Moq;
using Xunit;
using FIIT_folder.Application.Materials.Commands;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Enums;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Application.Materials.Commands;

/// <summary>
/// Tests for UploadMaterialCommandHandler.
/// Covers:
/// 1. Successful upload.
/// 2. Throws KeyNotFoundException when Subject is not found.
/// 3. Throws InvalidOperationException when MaterialType is not allowed.
/// </summary>
public class UploadMaterialCommandHandlerTests
{
    private readonly Mock<IMaterialMongoDB> _materialRepositoryMock;
    private readonly Mock<IFileStorageRepository> _fileStorageMock;
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly UploadMaterialCommandHandler _handler;

    public UploadMaterialCommandHandlerTests()
    {
        _materialRepositoryMock = new Mock<IMaterialMongoDB>();
        _fileStorageMock = new Mock<IFileStorageRepository>();
        _subjectRepositoryMock = new Mock<ISubjectRepository>();

        _handler = new UploadMaterialCommandHandler(
            _materialRepositoryMock.Object,
            _fileStorageMock.Object,
            _subjectRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldUploadMaterial_WhenRequestIsValid()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var stream = new MemoryStream();
        var command = new UploadMaterialCommand(
            SubjectId: subjectId,
            UserId: userId,
            FileName: "test.pdf",
            Year: 2023,
            Semester: 1,
            Description: "Test material",
            MaterialType: "Exam",
            Size: 1024,
            ContentType: "application/pdf",
            FileStream: stream
        );

        var subject = new Subject(
            new SubjectId(subjectId),
            new SubjectName("Math"),
            new SubjectSemester(1),
            new List<MaterialType> { MaterialType.Exam }
        );

        _subjectRepositoryMock.Setup(r => r.GetById(subjectId))
            .ReturnsAsync(subject);

        _fileStorageMock.Setup(s => s.SaveFile(
                It.IsAny<string>(), 
                It.IsAny<long>(), 
                It.IsAny<string>(), 
                It.IsAny<Stream>(), 
                It.IsAny<string>()))
            .ReturnsAsync("cloud/path/to/test.pdf");

        _materialRepositoryMock.Setup(r => r.CreateStudyMaterial(It.IsAny<StudyMaterial>()))
            .ReturnsAsync((StudyMaterial m) => m);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test.pdf", result.Name);
        
        _materialRepositoryMock.Verify(r => r.CreateStudyMaterial(It.IsAny<StudyMaterial>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFoundException_WhenSubjectNotFound()
    {
        // Arrange
        var command = new UploadMaterialCommand(
            SubjectId: Guid.NewGuid(),
            UserId: Guid.NewGuid(),
            FileName: "test.pdf",
            Year: 2023,
            Semester: 1,
            Description: "Test",
            MaterialType: "Exam",
            Size: 1024,
            ContentType: "application/pdf",
            FileStream: new MemoryStream()
        );
        
        _subjectRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Subject?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenMaterialTypeNotAllowed()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var command = new UploadMaterialCommand(
            SubjectId: subjectId,
            UserId: Guid.NewGuid(),
            FileName: "test.pdf",
            Year: 2023,
            Semester: 1,
            Description: "Test",
            MaterialType: "Colloquium", // Not in allowed list
            Size: 1024,
            ContentType: "application/pdf",
            FileStream: new MemoryStream()
        );

        var subject = new Subject(
            new SubjectId(subjectId),
            new SubjectName("Math"),
            new SubjectSemester(1),
            new List<MaterialType> { MaterialType.Exam } // Only Exam allowed
        );

        _subjectRepositoryMock.Setup(r => r.GetById(subjectId)).ReturnsAsync(subject);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("Colloquium", ex.Message);
    }
}

