using Moq;
using Xunit;
using FIIT_folder.Application.Favorites.Queries;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Favorites.Queries;

public class GetFavoritesHandlerTests
{
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IMaterialMongoDB> _materialRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMaterialRatingRepository> _ratingRepositoryMock;
    private readonly GetFavoritesHandler _handler;

    public GetFavoritesHandlerTests()
    {
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _materialRepositoryMock = new Mock<IMaterialMongoDB>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _ratingRepositoryMock = new Mock<IMaterialRatingRepository>();

        _handler = new GetFavoritesHandler(
            _favoriteRepositoryMock.Object,
            _materialRepositoryMock.Object,
            _userRepositoryMock.Object,
            _ratingRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFavorites_WithDetails()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var materialId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var query = new GetFavoritesQuery(userId);

        var favorite = new FavoriteMaterial(new UserId(userId), new StudyMaterialId(materialId));
        var material = new StudyMaterial(
            new MaterialName("Test.pdf"),
            new SubjectId(Guid.NewGuid()),
            new UserId(authorId),
            new StudyYear(2023),
            new Semester(1),
            "Description",
            new MaterialSize(1024),
            MaterialType.Exam,
            new ResourceLocation("path/to/file.pdf"));

        // Initialize protected/private Id for testing if needed, relying on public getters which should work if entity instantiated correctly
        // Or setup mocks to match IDs. The entity constructor generates new ID.
        // We need to ensure material.Id matches favorite.MaterialId for integration logic check?
        // No, favorite contains MaterialId. 
        // Mock repositories:

        _favoriteRepositoryMock.Setup(r => r.GetMaterialsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FavoriteMaterial> { favorite });

        _materialRepositoryMock.Setup(r => r.GetByIdStudyMaterial(materialId))
            .ReturnsAsync(material);

        _userRepositoryMock.Setup(r => r.GetByIdAsync(authorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User(UserId.New(), new Login("AuthorName"), new PasswordHash("hash"), UserRole.Student));

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((5, 1));

        _ratingRepositoryMock.Setup(r => r.GetByUserAndMaterialAsync(It.IsAny<UserId>(), It.IsAny<StudyMaterialId>()))
            .ReturnsAsync(new MaterialRating
            {
                UserId = new UserId(userId),
                MaterialId = new StudyMaterialId(material.Id.Value),
                Rating = RatingType.Like
            });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal("Test.pdf", dto.Name);
        Assert.Equal("AuthorName", dto.AuthorName);
        Assert.Equal(5, dto.LikesCount);
        Assert.Equal("Like", dto.CurrentUserRating);
    }

    [Fact]
    public async Task Handle_ShouldHandleMissingMaterial()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetFavoritesQuery(userId);
        var favorite = new FavoriteMaterial(new UserId(userId), new StudyMaterialId(Guid.NewGuid()));

        _favoriteRepositoryMock.Setup(r => r.GetMaterialsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FavoriteMaterial> { favorite });

        _materialRepositoryMock.Setup(r => r.GetByIdStudyMaterial(It.IsAny<Guid>()))
            .ReturnsAsync((StudyMaterial?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal("Unknown Material", result.First().Name);
    }
}
