using Moq;
using Xunit;
using FIIT_folder.Application.Materials.Queries;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Tests.Application.Materials.Queries;

/// <summary>
/// Tests for GetMaterialByIdQueryHandler.
/// </summary>
public class GetMaterialByIdQueryHandlerTests
{
    private readonly Mock<IMaterialMongoDB> _materialRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IMaterialRatingRepository> _ratingRepositoryMock;
    private readonly GetMaterialByIdQueryHandler _handler;

    public GetMaterialByIdQueryHandlerTests()
    {
        _materialRepositoryMock = new Mock<IMaterialMongoDB>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _ratingRepositoryMock = new Mock<IMaterialRatingRepository>();

        _handler = new GetMaterialByIdQueryHandler(
            _materialRepositoryMock.Object,
            _userRepositoryMock.Object,
            _favoriteRepositoryMock.Object,
            _ratingRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenMaterialNotFound()
    {
        // Arrange
        var query = new GetMaterialByIdQuery(Guid.NewGuid(), null);
        _materialRepositoryMock.Setup(r => r.GetByIdStudyMaterial(It.IsAny<Guid>()))
            .ReturnsAsync((StudyMaterial?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnMaterialDto_WhenFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Create a real StudyMaterial
        var material = new StudyMaterial(
            new MaterialName("Test.pdf"),
            new SubjectId(Guid.NewGuid()),
            new UserId(userId),
            new StudyYear(2023),
            new Semester(1),
            "Description",
            new MaterialSize(100),
            MaterialType.Exam,
            new ResourceLocation("path/to/file.pdf"));
        
        // Ensure mock returns this material when asked for ANY id (or we could use material.Id.Value)
        _materialRepositoryMock.Setup(r => r.GetByIdStudyMaterial(It.IsAny<Guid>()))
            .ReturnsAsync(material);

        var author = new User(new UserId(userId), Login.Create("testuser"), new PasswordHash("hash"), UserRole.Student);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(author);

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(material.Id.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync((5, 1)); // 5 likes, 1 dislike

        var query = new GetMaterialByIdQuery(material.Id.Value, null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(material.Id.Value, result!.Id);
        Assert.Equal("testuser", result.AuthorName);
        Assert.Equal(5, result.LikesCount);
        Assert.Equal(1, result.DislikesCount);
        Assert.False(result.IsFavorite); // User ID not provided in query
    }

    [Fact]
    public async Task Handle_ShouldMarkAsFavorite_WhenUserHasFavorited()
    {
         // Arrange
        var userId = Guid.NewGuid();
        
        var material = new StudyMaterial(
            new MaterialName("Test.pdf"),
            new SubjectId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new StudyYear(2023),
            new Semester(1),
            "Desc",
            new MaterialSize(100),
            MaterialType.Exam,
            new ResourceLocation("path/to/file.pdf"));

        _materialRepositoryMock.Setup(r => r.GetByIdStudyMaterial(It.IsAny<Guid>()))
            .ReturnsAsync(material);

        var favorite = new FavoriteMaterial(new UserId(userId), material.Id);

        _favoriteRepositoryMock.Setup(r => r.GetMaterialsByUserIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FavoriteMaterial> { favorite });

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((0, 0));

        var query = new GetMaterialByIdQuery(material.Id.Value, userId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result!.IsFavorite);
    }
}

