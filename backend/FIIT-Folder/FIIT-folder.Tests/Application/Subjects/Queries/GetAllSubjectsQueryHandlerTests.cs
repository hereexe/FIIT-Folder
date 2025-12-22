using Moq;
using Xunit;
using FIIT_folder.Application.Subjects.Queries;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Subjects.Queries;

public class GetAllSubjectsQueryHandlerTests
{
    private readonly Mock<ISubjectRepository> _repositoryMock;
    private readonly GetAllSubjectsQueryHandler _handler;

    public GetAllSubjectsQueryHandlerTests()
    {
        _repositoryMock = new Mock<ISubjectRepository>();
        _handler = new GetAllSubjectsQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllSubjects()
    {
        // Arrange
        var query = new GetAllSubjectsQuery();
        var subjects = new List<Subject>
        {
            new Subject(new SubjectId(Guid.NewGuid()), new SubjectName("Math"), new SubjectSemester(1), new List<MaterialType>()),
            new Subject(new SubjectId(Guid.NewGuid()), new SubjectName("Physics"), new SubjectSemester(2), new List<MaterialType>())
        };

        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(subjects);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, s => s.Name == "Math");
        Assert.Contains(result, s => s.Name == "Physics");
    }
}
