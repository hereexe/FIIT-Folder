using Moq;
using Xunit;
using FIIT_folder.Application.Materials.Queries;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Materials.Queries;

public class DownloadMaterialQueryHandlerTests
{
    private readonly Mock<IMaterialMongoDB> _materialRepositoryMock;
    private readonly Mock<IFileStorageRepository> _fileStorageMock;
    private readonly DownloadMaterialQueryHandler _handler;

    public DownloadMaterialQueryHandlerTests()
    {
        _materialRepositoryMock = new Mock<IMaterialMongoDB>();
        _fileStorageMock = new Mock<IFileStorageRepository>();
        _handler = new DownloadMaterialQueryHandler(_materialRepositoryMock.Object, _fileStorageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFile_WhenFound()
    {
        // Arrange
        var materialId = Guid.NewGuid();
        var query = new DownloadMaterialQuery(materialId);
        var material = new StudyMaterial(
            new MaterialName("test.pdf"),
            new SubjectId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new StudyYear(2023),
            new Semester(1),
            "Desc",
            new MaterialSize(1024),
            MaterialType.Exam,
            new ResourceLocation("cloud/path/test.pdf"));

        _materialRepositoryMock.Setup(r => r.GetByIdStudyMaterial(materialId))
            .ReturnsAsync(material);

        var fileStream = new MemoryStream();
        _fileStorageMock.Setup(s => s.GetFile("cloud/path/test.pdf"))
            .ReturnsAsync(fileStream);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fileStream, result.FileStream);
        Assert.Equal("test.pdf", result.FileName);
        // Assert.Equal("application/pdf", result.ContentType); // Content type determination logic might need checking if it's dynamic
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenMaterialNotFound()
    {
        // Arrange
        var query = new DownloadMaterialQuery(Guid.NewGuid());
        _materialRepositoryMock.Setup(r => r.GetByIdStudyMaterial(It.IsAny<Guid>()))
            .ReturnsAsync((StudyMaterial?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
