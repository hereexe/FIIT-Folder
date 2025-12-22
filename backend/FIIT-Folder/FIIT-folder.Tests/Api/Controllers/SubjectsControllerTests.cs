using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FIIT_folder.Api.Controllers;
using FIIT_folder.Api.Models;
using FIIT_folder.Application.Subjects.Commands;
using FIIT_folder.Application.Subjects.Queries;
using FIIT_folder.Application.DTOs;

namespace FIIT_folder.Tests.Api.Controllers;

/// <summary>
/// Tests for SubjectsController.
/// </summary>
public class SubjectsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly SubjectsController _controller;

    public SubjectsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new SubjectsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreated_WithSubjectResponse()
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            Name = "Mathematics",
            Semester = 1,
            MaterialTypes = new List<string> { "Exam" }
        };

        var subjectDto = new SubjectDto
        {
            Id = Guid.NewGuid(),
            Name = "Mathematics",
            Semester = 1,
            MaterialTypes = new List<MaterialTypeDto>
            {
                new MaterialTypeDto { Value = "Exam", DisplayName = "Экзамены" }
            }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateSubjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subjectDto);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.NotNull(createdResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<CreateSubjectCommand>(c => c.Name == request.Name && c.Semester == request.Semester),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithSubjectsList()
    {
        // Arrange
        var subjects = new List<SubjectDto>
        {
            new SubjectDto { Id = Guid.NewGuid(), Name = "Math", Semester = 1 },
            new SubjectDto { Id = Guid.NewGuid(), Name = "Physics", Semester = 2 }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllSubjectsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subjects);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(It.IsAny<GetAllSubjectsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WithSubject()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var subjectDto = new SubjectDto
        {
            Id = subjectId,
            Name = "Mathematics",
            Semester = 1,
            MaterialTypes = new List<MaterialTypeDto>()
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetSubjectByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subjectDto);

        // Act
        var result = await _controller.GetById(subjectId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetSubjectByIdQuery>(q => q.Id == subjectId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent()
    {
        // Arrange
        var subjectId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteSubjectCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(subjectId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<DeleteSubjectCommand>(c => c.Id == subjectId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetWithMaterials_ShouldReturnOk_WithSubjectAndMaterials()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var subjectWithMaterials = new SubjectWithMaterialsDto
        {
            Id = subjectId,
            Name = "Mathematics",
            MaterialGroups = new List<MaterialGroupDto>()
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetSubjectWithMaterialsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subjectWithMaterials);

        // Act
        var result = await _controller.GetWithMaterials(subjectId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        
        _mediatorMock.Verify(m => m.Send(
            It.Is<GetSubjectWithMaterialsQuery>(q => q.SubjectId == subjectId),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
