using Moq;
using Xunit;
using FIIT_folder.Application.Subjects.Queries;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Subjects.Queries;

public class GetSubjectWithMaterialsQueryHandlerTests
{
    private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
    private readonly Mock<IMaterialMongoDB> _materialRepositoryMock;
    private readonly GetSubjectWithMaterialsQueryHandler _handler;

    public GetSubjectWithMaterialsQueryHandlerTests()
    {
        _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _materialRepositoryMock = new Mock<IMaterialMongoDB>();
        _handler = new GetSubjectWithMaterialsQueryHandler(_subjectRepositoryMock.Object, _materialRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSubjectWithMaterials_WhenFound()
    {
        // Arrange
        var targetId = Guid.NewGuid();
        var query = new GetSubjectWithMaterialsQuery(targetId);

        var targetSubject = new Subject(new SubjectId(targetId), new SubjectName("Math"), new SubjectSemester(1), new List<MaterialType> { MaterialType.Exam });
        
        // Another semester of the same subject
        var subjectSem2 = new Subject(new SubjectId(Guid.NewGuid()), new SubjectName("Math"), new SubjectSemester(2), new List<MaterialType> { MaterialType.Exam });

        _subjectRepositoryMock.Setup(r => r.GetById(targetId)).ReturnsAsync(targetSubject);
        _subjectRepositoryMock.Setup(r => r.GetByName("Math")).ReturnsAsync(new List<Subject> { targetSubject, subjectSem2 });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(targetId, result.Id);
        Assert.Equal("Math", result.Name);
        Assert.Single(result.MaterialGroups); // Both have Exam, so 1 group
        
        var group = result.MaterialGroups.First();
        Assert.Equal("Exam", group.RawType);
        Assert.Equal(2, group.Items.Count); // 2 items (sem 1 and sem 2)
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenSubjectNotFound()
    {
        // Arrange
        var query = new GetSubjectWithMaterialsQuery(Guid.NewGuid());
        _subjectRepositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Subject?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
