using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FIIT_folder.Api.Controllers;
using FIIT_folder.Api.Models;
using FIIT_folder.Application.Ratings.Commands;
using FIIT_folder.Application.Ratings.Queries;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Enums;
using System.Security.Claims;

namespace FIIT_folder.Tests.Api.Controllers;

/// <summary>
/// Tests for RatingController.
/// </summary>
public class RatingControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RatingController _controller;

    public RatingControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RatingController(_mediatorMock.Object);
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
    public async Task Rate_ShouldReturnOk_WhenSuccessful()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var materialId = Guid.NewGuid();
        SetupAuthenticatedUser(userId);

        var request = new RateRequest { RatingType = RatingType.Like };
        var ratingResult = new RatingResultDto { LikesCount = 1, DislikesCount = 0, UserRating = RatingType.Like };

        _mediatorMock.Setup(m => m.Send(It.IsAny<RateMaterialCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ratingResult);

        // Act
        var result = await _controller.Rate(materialId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<RateMaterialCommand>(c => 
                c.MaterialId == materialId && 
                c.UserId == userId &&
                c.RatingType == request.RatingType),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetRating_ShouldReturnOk_WithRatingInfo()
    {
        // Arrange
        var materialId = Guid.NewGuid();
        var ratingInfo = new RatingResultDto { LikesCount = 5, DislikesCount = 2 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetRatingQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ratingInfo);

        // Act
        var result = await _controller.GetRating(materialId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetRatingQuery>(q => q.MaterialId == materialId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUserIdFromToken_ShouldThrow_WhenNoAuthenticatedUser()
    {
        // Arrange
        var materialId = Guid.NewGuid();
        var request = new RateRequest { RatingType = RatingType.Like };
        
        // No user setup - controller has default empty context
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.Rate(materialId, request));
    }
}
