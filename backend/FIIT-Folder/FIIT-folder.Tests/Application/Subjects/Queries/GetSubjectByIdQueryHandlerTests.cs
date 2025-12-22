using Moq;
using Xunit;
using FIIT_folder.Application.Subjects.Queries;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Subjects.Queries;

public class GetSubjectByIdQueryHandlerTests
{
    private readonly Mock<ISubjectRepository> _repositoryMock;
    private readonly GetSubjectByIdQueryHandler _handler;

    public GetSubjectByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<ISubjectRepository>();
        _handler = new GetSubjectByIdQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSubject_WhenFound()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var query = new GetSubjectByIdQuery(subjectId);
        var subject = new Subject(new SubjectId(subjectId), new SubjectName("Math"), new SubjectSemester(1), new List<MaterialType> { MaterialType.Exam });

        _repositoryMock.Setup(r => r.GetById(subjectId)).ReturnsAsync(subject);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(subjectId, result.Id);
        Assert.Equal("Math", result.Name);
        Assert.Single(result.MaterialTypes);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var query = new GetSubjectByIdQuery(Guid.NewGuid());
        _repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Subject?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
