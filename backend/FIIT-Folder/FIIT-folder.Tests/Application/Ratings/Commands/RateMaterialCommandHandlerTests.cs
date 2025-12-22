using Moq;
using Xunit;
using FIIT_folder.Application.Ratings.Commands;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;
using FIIT_folder.Domain.Entities;

namespace FIIT_folder.Tests.Application.Ratings.Commands;

public class RateMaterialCommandHandlerTests
{
    private readonly Mock<IMaterialRatingRepository> _ratingRepositoryMock;
    private readonly RateMaterialCommandHandler _handler;

    public RateMaterialCommandHandlerTests()
    {
        _ratingRepositoryMock = new Mock<IMaterialRatingRepository>();
        _handler = new RateMaterialCommandHandler(_ratingRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddRating_WhenDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var materialId = Guid.NewGuid();
        var command = new RateMaterialCommand(materialId, userId, RatingType.Like);

        var newRating = new MaterialRating
        {
            UserId = new UserId(userId),
            MaterialId = new StudyMaterialId(materialId),
            Rating = RatingType.Like
        };

        // First call returns null (no existing rating), second call returns the new rating (after add)
        _ratingRepositoryMock.SetupSequence(r => r.GetByUserAndMaterialAsync(It.IsAny<UserId>(), It.IsAny<StudyMaterialId>()))
            .ReturnsAsync((MaterialRating?)null)
            .ReturnsAsync(newRating);

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((1, 0));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(RatingType.Like, result.UserRating);
        Assert.Equal(1, result.LikesCount);
        
        _ratingRepositoryMock.Verify(r => r.AddAsync(It.IsAny<MaterialRating>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldUpdateRating_WhenExistsAndDifferent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var materialId = Guid.NewGuid();
        var command = new RateMaterialCommand(materialId, userId, RatingType.Dislike);
        var existingRating = new MaterialRating
        {
            UserId = new UserId(userId),
            MaterialId = new StudyMaterialId(materialId),
            Rating = RatingType.Like
        };

        _ratingRepositoryMock.Setup(r => r.GetByUserAndMaterialAsync(It.IsAny<UserId>(), It.IsAny<StudyMaterialId>()))
            .ReturnsAsync(existingRating);

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((0, 1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(RatingType.Dislike, result.UserRating);
        
        _ratingRepositoryMock.Verify(r => r.UpdateAsync(
            It.Is<MaterialRating>(m => m.Rating == RatingType.Dislike)), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldRemoveRating_WhenSameType()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var materialId = Guid.NewGuid();
        var command = new RateMaterialCommand(materialId, userId, RatingType.Like);
        var existingRating = new MaterialRating
        {
            UserId = new UserId(userId),
            MaterialId = new StudyMaterialId(materialId),
            Rating = RatingType.Like
        };

        _ratingRepositoryMock.SetupSequence(r => r.GetByUserAndMaterialAsync(It.IsAny<UserId>(), It.IsAny<StudyMaterialId>()))
            .ReturnsAsync(existingRating)
            .ReturnsAsync((MaterialRating?)null);

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((0, 0));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result.UserRating);
        
        _ratingRepositoryMock.Verify(r => r.DeleteAsync(
            It.IsAny<Guid>(), 
            It.IsAny<Guid>()), Times.Once);
    }
}
