using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FIIT_folder.Api.Controllers;
using FIIT_folder.Api.Models;
using FIIT_folder.Application.Materials.Queries;
using FIIT_folder.Application.Materials.Commands;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;
using System.Security.Claims;

namespace FIIT_folder.Tests.Api.Controllers;

/// <summary>
/// Tests for MaterialsController.
/// </summary>
public class MaterialsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMaterialMongoDB> _materialRepositoryMock;
    private readonly MaterialsController _controller;

    public MaterialsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _materialRepositoryMock = new Mock<IMaterialMongoDB>();
        _controller = new MaterialsController(_mediatorMock.Object, _materialRepositoryMock.Object);
        
        // Setup HttpContext with Request properties
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost");
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetBySubject_ShouldReturnOk_WithMaterials()
    {
        // Arrange
        var request = new GetMaterialsRequest
        {
            SubjectId = Guid.NewGuid()
        };

        var materials = new List<MaterialDto>
        {
            new MaterialDto
            {
                Id = Guid.NewGuid(),
                Name = "Test.pdf",
                SubjectId = request.SubjectId!.Value,
                Year = 2023,
                Semester = 1,
                MaterialType = "Exam",
                Size = 1024
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMaterialsBySubjectQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(materials);

        // Act
        var result = await _controller.GetBySubject(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenMaterialExists()
    {
        // Arrange
        var materialId = Guid.NewGuid();
        var materialDto = new MaterialDto
        {
            Id = materialId,
            Name = "Test.pdf",
            SubjectId = Guid.NewGuid(),
            Year = 2023,
            Semester = 1,
            MaterialType = "Exam",
            Size = 1024
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMaterialByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(materialDto);

        // Act
        var result = await _controller.GetById(materialId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenMaterialDoesNotExist()
    {
        // Arrange
        var materialId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetMaterialByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((MaterialDto?)null);

        // Act
        var result = await _controller.GetById(materialId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var materialId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteMaterialCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(materialId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<DeleteMaterialCommand>(c => c.Id == materialId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Download_ShouldReturnNotFound_WhenMaterialDoesNotExist()
    {
        // Arrange
        var materialId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DownloadMaterialQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DownloadMaterialResult?)null);

        // Act
        var result = await _controller.Download(materialId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Download_ShouldReturnFile_WhenMaterialExists()
    {
        // Arrange
        var materialId = Guid.NewGuid();
        var downloadResult = new DownloadMaterialResult
        {
            FileStream = new MemoryStream(new byte[] { 1, 2, 3 }),
            ContentType = "application/pdf",
            FileName = "test.pdf"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<DownloadMaterialQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(downloadResult);

        // Act
        var result = await _controller.Download(materialId);

        // Assert
        Assert.IsType<FileStreamResult>(result);
    }
}
