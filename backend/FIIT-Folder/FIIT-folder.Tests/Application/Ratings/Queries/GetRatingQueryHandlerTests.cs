using Moq;
using Xunit;
using FIIT_folder.Application.Ratings.Queries;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Tests.Application.Ratings.Queries;

public class GetRatingQueryHandlerTests
{
    private readonly Mock<IMaterialRatingRepository> _ratingRepositoryMock;
    private readonly GetRatingQueryHandler _handler;

    public GetRatingQueryHandlerTests()
    {
        _ratingRepositoryMock = new Mock<IMaterialRatingRepository>();
        _handler = new GetRatingQueryHandler(_ratingRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnRatingCounts()
    {
        // Arrange
        var materialId = Guid.NewGuid();
        var query = new GetRatingQuery(materialId);

        _ratingRepositoryMock.Setup(r => r.GetRatingCountsAsync(materialId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((10, 5));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(10, result.LikesCount);
        Assert.Equal(5, result.DislikesCount);
        Assert.Null(result.UserRating);
    }
}
