using FakeItEasy;
using Xunit;
using FIIT_folder.Application.Subjects.Commands;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Enums;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Tests.Application.Subjects.Commands;

/// <summary>
/// Tests for CreateSubjectCommandHandler using FakeItEasy.
/// </summary>
public class CreateSubjectCommandHandlerTests
{
    private readonly ISubjectRepository _subjectRepositoryFake;
    private readonly CreateSubjectCommandHandler _handler;

    public CreateSubjectCommandHandlerTests()
    {
        _subjectRepositoryFake = A.Fake<ISubjectRepository>();
        _handler = new CreateSubjectCommandHandler(_subjectRepositoryFake);
    }

    [Fact]
    public async Task Handle_ShouldCreateSubject_WhenRequestIsValid()
    {
        // Arrange
        var command = new CreateSubjectCommand(
            Name: "Mathematics",
            Semester: 1,
            MaterialTypes: new List<string> { "Exam", "Colloquium" }
        );

        A.CallTo(() => _subjectRepositoryFake.ExistsByName(command.Name)).Returns(false);
        
        // Mock Create to return the passed subject (or a similar one)
        A.CallTo(() => _subjectRepositoryFake.Create(A<Subject>.Ignored))
            .ReturnsLazily((Subject s) => Task.FromResult(s));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Mathematics", result.Name);
        Assert.Equal(1, result.Semester);
        Assert.Contains(result.MaterialTypes, mt => mt.Value == "Exam");
        Assert.Contains(result.MaterialTypes, mt => mt.Value == "Colloquium");

        A.CallTo(() => _subjectRepositoryFake.Create(A<Subject>.That.Matches(s => s.Name.Value == "Mathematics")))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenSubjectAlreadyExists()
    {
        // Arrange
        var command = new CreateSubjectCommand(
            Name: "Math",
            Semester: 1,
            MaterialTypes: new List<string> { "Exam" }
        );
        
        A.CallTo(() => _subjectRepositoryFake.ExistsByName(command.Name)).Returns(true);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Contains("уже существует", ex.Message);
        
        A.CallTo(() => _subjectRepositoryFake.Create(A<Subject>.Ignored)).MustNotHaveHappened();
    }
}

