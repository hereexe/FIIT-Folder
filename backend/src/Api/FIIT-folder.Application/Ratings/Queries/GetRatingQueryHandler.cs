using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;
using MediatR;

namespace FIIT_folder.Application.Ratings.Queries;

public class GetRatingQueryHandler : IRequestHandler<GetRatingQuery, RatingResultDto>
{
    private readonly IMaterialRatingRepository _ratingRepository;

    public GetRatingQueryHandler(IMaterialRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task<RatingResultDto> Handle(GetRatingQuery request, CancellationToken
        cancellationToken)
    {
        var (likes, dislikes) = await _ratingRepository.GetRatingCountsAsync(request.MaterialId,
            cancellationToken);

        return new RatingResultDto
        {
            LikesCount = likes,
            DislikesCount = dislikes,
            UserRating = null
        };
    }
}