using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using MediatR;

namespace FIIT_folder.Application.Ratings.Commands;

public class RateMaterialCommandHandler : IRequestHandler<RateMaterialCommand, RatingResultDto>
{
    private readonly IMaterialRatingRepository _ratingRepository;

    public RateMaterialCommandHandler(IMaterialRatingRepository ratingRepository)
    {
        _ratingRepository = ratingRepository;
    }

    public async Task<RatingResultDto> Handle(RateMaterialCommand request, CancellationToken cancellationToken)
    {
        var existingRating = await _ratingRepository.GetByUserAndMaterialAsync(
            new UserId(request.UserId), 
            new StudyMaterialId(request.MaterialId));

        if (existingRating != null)
        {
            if (existingRating.Rating == request.RatingType)
            {
                // Toggle off
                await _ratingRepository.DeleteAsync(request.MaterialId, request.UserId);
            }
            else
            {
                // Change rating
                existingRating.Rating = request.RatingType;
                existingRating.UpdatedAt = DateTime.UtcNow;
                await _ratingRepository.UpdateAsync(existingRating);
            }
        }
        else
        {
            // New rating
            var rating = new MaterialRating
            {
                Id = Guid.NewGuid(),
                MaterialId = new StudyMaterialId(request.MaterialId),
                UserId = new UserId(request.UserId),
                Rating = request.RatingType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _ratingRepository.AddAsync(rating);
        }

        var (likes, dislikes) = await _ratingRepository.GetRatingCountsAsync(request.MaterialId, cancellationToken);
        var currentRating = await _ratingRepository.GetByUserAndMaterialAsync(
            new UserId(request.UserId), 
            new StudyMaterialId(request.MaterialId));

        return new RatingResultDto
        {
            LikeCount = likes,
            DislikeCount = dislikes,
            UserRating = currentRating?.Rating
        };
    }
}
