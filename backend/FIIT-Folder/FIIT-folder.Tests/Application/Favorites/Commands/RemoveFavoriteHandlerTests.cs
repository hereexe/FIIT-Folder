using Moq;
using Xunit;
using FIIT_folder.Application.Favorites.Commands;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Application.Favorites.Commands;

public class RemoveFavoriteHandlerTests
{
    private readonly Mock<IFavoriteRepository> _repositoryMock;
    private readonly RemoveFavoriteHandler _handler;

    public RemoveFavoriteHandlerTests()
    {
        _repositoryMock = new Mock<IFavoriteRepository>();
        _handler = new RemoveFavoriteHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveFavorite_WhenCalled()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var command = new RemoveFavoriteCommand(userId, itemId);

        _repositoryMock.Setup(r => r.RemoveMaterialAsync(It.IsAny<UserId>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.RemoveMaterialAsync(
            It.Is<UserId>(u => u.Value == userId),
            itemId,
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
