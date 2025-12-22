using Moq;
using Xunit;
using FIIT_folder.Application.Materials.Commands;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Materials.Commands;

public class DeleteMaterialCommandHandlerTests
{
    private readonly Mock<IMaterialMongoDB> _repositoryMock;
    private readonly Mock<IFileStorageRepository> _fileStorageMock;
    private readonly DeleteMaterialCommandHandler _handler;

    public DeleteMaterialCommandHandlerTests()
    {
        _repositoryMock = new Mock<IMaterialMongoDB>();
        _fileStorageMock = new Mock<IFileStorageRepository>();
        _handler = new DeleteMaterialCommandHandler(_repositoryMock.Object, _fileStorageMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteMaterial_WhenCalled()
    {
        // Arrange
        var materialId = Guid.NewGuid();
        var command = new DeleteMaterialCommand(materialId);

        _repositoryMock.Setup(r => r.GetByIdStudyMaterial(materialId))
            .ReturnsAsync(new StudyMaterial(new MaterialName("test"), new SubjectId(Guid.NewGuid()), new UserId(Guid.NewGuid()), new StudyYear(2022), new Semester(1), "desc", new MaterialSize(100), MaterialType.Exam, new ResourceLocation("path.pdf")));

        _repositoryMock.Setup(r => r.DeleteStudyMaterial(materialId)).ReturnsAsync(true);
        _fileStorageMock.Setup(f => f.DeleteFile(It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.DeleteStudyMaterial(materialId), Times.Once);
    }
}
