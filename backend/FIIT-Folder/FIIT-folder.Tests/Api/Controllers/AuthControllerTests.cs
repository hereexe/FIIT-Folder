using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FIIT_folder.Api.Controllers;
using FIIT_folder.Api.Models;
using FIIT_folder.Application.Users.Commands;

namespace FIIT_folder.Tests.Api.Controllers;

/// <summary>
/// Tests for AuthController.
/// </summary>
    public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new AuthController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WithUserId()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Password = "password123"
        };
        var expectedUserId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUserId);

        // Act
        var result = await _controller.Register(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<RegisterUserCommand>(c => c.Username == request.Username && c.Password == request.Password),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WithToken()
    {
        // Arrange
        var request = new LoginRequest
        {
            Username = "testuser",
            Password = "password123"
        };
        var expectedToken = "jwt_token_here";

        _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedToken);

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<LoginUserCommand>(c => c.Username == request.Username && c.Password == request.Password),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
