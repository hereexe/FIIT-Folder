using Moq;
using Xunit;
using FIIT_folder.Application.Materials.Queries;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Materials.Queries;

public class GetMaterialsBySubjectQueryHandlerTests
{
    private readonly Mock<IMaterialMongoDB> _materialRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IMaterialRatingRepository> _ratingRepositoryMock;
    private readonly GetMaterialsBySubjectQueryHandler _handler;

    public GetMaterialsBySubjectQueryHandlerTests()
    {
        _materialRepositoryMock = new Mock<IMaterialMongoDB>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _ratingRepositoryMock = new Mock<IMaterialRatingRepository>();

        _handler = new GetMaterialsBySubjectQueryHandler(
            _materialRepositoryMock.Object,
            _userRepositoryMock.Object,
            _favoriteRepositoryMock.Object,
            _ratingRepositoryMock.Object);
            
        _favoriteRepositoryMock.Setup(r => r.GetMaterialsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FavoriteMaterial>());
    }

    [Fact]
    public async Task Handle_ShouldReturnMaterials_WhenFound()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        // Query(SubjectId, UserId, Semester, Year, SearchText)
        var query = new GetMaterialsBySubjectQuery(subjectId, userId);

        var material = new StudyMaterial(
            new MaterialName("test.pdf"),
            new SubjectId(subjectId),
            new UserId(userId), // Author ID
            new StudyYear(2023),
            new Semester(1),
            "Desc",
            new MaterialSize(1024),
            MaterialType.Exam,
            new ResourceLocation("cloud/path/test.pdf"));

        _materialRepositoryMock.Setup(r => r.GetBySubjectId(subjectId))
            .ReturnsAsync(new List<StudyMaterial> { material });

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((5, 0));

        _ratingRepositoryMock.Setup(r => r.GetByUserAndMaterialAsync(It.IsAny<UserId>(), It.IsAny<StudyMaterialId>()))
            .ReturnsAsync((MaterialRating?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal("test.pdf", dto.Name);
        Assert.Equal(5, dto.LikesCount);
        Assert.Equal("Exam", dto.MaterialType); // Enum.ToString()
    }
}
