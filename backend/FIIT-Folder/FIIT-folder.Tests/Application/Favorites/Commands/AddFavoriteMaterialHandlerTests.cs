using Moq;
using Xunit;
using FIIT_folder.Application.Favorites.Commands;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Application.Favorites.Commands;

public class AddFavoriteMaterialHandlerTests
{
    private readonly Mock<IFavoriteRepository> _repositoryMock;
    private readonly AddFavoriteMaterialHandler _handler;

    public AddFavoriteMaterialHandlerTests()
    {
        _repositoryMock = new Mock<IFavoriteRepository>();
        _handler = new AddFavoriteMaterialHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddFavorite_WhenNotExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var materialId = Guid.NewGuid();
        var command = new AddFavoriteMaterialCommand(userId, materialId);

        _repositoryMock.Setup(r => r.GetMaterialsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FavoriteMaterial>());

        _repositoryMock.Setup(r => r.AddMaterialAsync(It.IsAny<FavoriteMaterial>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _repositoryMock.Verify(r => r.AddMaterialAsync(
            It.Is<FavoriteMaterial>(f => f.UserId.Value == userId && f.MaterialId.Value == materialId), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnExistingId_WhenAlreadyExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var materialId = Guid.NewGuid();
        var command = new AddFavoriteMaterialCommand(userId, materialId);

        var existingFavorite = new FavoriteMaterial(new UserId(userId), new StudyMaterialId(materialId));

        _repositoryMock.Setup(r => r.GetMaterialsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FavoriteMaterial> { existingFavorite });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(existingFavorite.Id, result);
        _repositoryMock.Verify(r => r.AddMaterialAsync(It.IsAny<FavoriteMaterial>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
