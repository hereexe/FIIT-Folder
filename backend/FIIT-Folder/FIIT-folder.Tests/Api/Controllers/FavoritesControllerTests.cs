using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FIIT_folder.Api.Controllers;
using FIIT_folder.Application.Favorites.Commands;
using FIIT_folder.Application.Favorites.Queries;
using FIIT_folder.Application.DTOs;
using System.Security.Claims;

namespace FIIT_folder.Tests.Api.Controllers;

/// <summary>
/// Tests for FavoritesController.
/// </summary>
public class FavoritesControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly FavoritesController _controller;
    private readonly Guid _userId;

    public FavoritesControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new FavoritesController(_mediatorMock.Object);
        _userId = Guid.NewGuid();
        
        SetupAuthenticatedUser(_userId);
    }

    private void SetupAuthenticatedUser(Guid userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task AddMaterial_ShouldReturnOk_WhenSuccessful()
    {
        // Arrange
        var request = new AddMaterialRequest(Guid.NewGuid());
        var favoriteId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<AddFavoriteMaterialCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(favoriteId);

        // Act
        var result = await _controller.AddMaterial(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(favoriteId, okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<AddFavoriteMaterialCommand>(c => 
                c.UserId == _userId && 
                c.MaterialId == request.MaterialId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Remove_ShouldReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // RemoveFavoriteCommand returns no value (IRequest without TResult)
        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoveFavoriteCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Remove(itemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<RemoveFavoriteCommand>(c => c.UserId == _userId && c.ItemId == itemId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetFavorites_ShouldReturnOk_WithFavoritesList()
    {
        // Arrange
        var favorites = new List<FavoriteMaterialDto>
        {
            new FavoriteMaterialDto { Id = Guid.NewGuid(), MaterialId = Guid.NewGuid() },
            new FavoriteMaterialDto { Id = Guid.NewGuid(), MaterialId = Guid.NewGuid() }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFavoritesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(favorites);

        // Act
        var result = await _controller.GetFavorites();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetFavoritesQuery>(q => q.UserId == _userId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUserIdFromToken_ShouldThrow_WhenNoAuthenticatedUser()
    {
        // Arrange
        // Setup empty context without user
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.GetFavorites());
    }
}
