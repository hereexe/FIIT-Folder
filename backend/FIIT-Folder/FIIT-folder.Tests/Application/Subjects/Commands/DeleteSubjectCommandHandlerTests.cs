using Moq;
using Xunit;
using FIIT_folder.Application.Subjects.Commands;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Subjects.Commands;

public class DeleteSubjectCommandHandlerTests
{
    private readonly Mock<ISubjectRepository> _repositoryMock;
    private readonly DeleteSubjectCommandHandler _handler;

    public DeleteSubjectCommandHandlerTests()
    {
        _repositoryMock = new Mock<ISubjectRepository>();
        _handler = new DeleteSubjectCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteSubject_WhenCalled()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var command = new DeleteSubjectCommand(subjectId);
        var subject = new Subject(
            new SubjectId(subjectId),
            new SubjectName("Test Subject"),
            new SubjectSemester(1),
            new[] { MaterialType.Exam });

        _repositoryMock.Setup(r => r.GetById(subjectId)).ReturnsAsync(subject);
        _repositoryMock.Setup(r => r.Delete(subjectId)).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.Delete(subjectId), Times.Once);
    }
}
