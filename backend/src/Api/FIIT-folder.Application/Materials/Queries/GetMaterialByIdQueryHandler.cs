using MediatR;
using FIIT_folder.Application.DTOs;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Materials.Queries;

public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDto?>
{
    private readonly IMaterialMongoDB _materialRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMaterialRatingRepository _ratingRepository;

    public GetMaterialByIdQueryHandler(
        IMaterialMongoDB materialRepository,
        IUserRepository userRepository,
        IFavoriteRepository favoriteRepository,
        IMaterialRatingRepository ratingRepository)
    {
        _materialRepository = materialRepository;
        _userRepository = userRepository;
        _favoriteRepository = favoriteRepository;
        _ratingRepository = ratingRepository;
    }

    public async Task<MaterialDto?> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        var material = await _materialRepository.GetByIdStudyMaterial(request.Id);

        if (material == null)
            return null;

        var authorName = "Unknown";
        var user = await _userRepository.GetByIdAsync(material.UserId.Value, cancellationToken);
        if (user != null)
        {
            authorName = user.Login.Value;
        }

        var isFavorite = false;
        if (request.UserId.HasValue)
        {
            var favorites = await _favoriteRepository.GetMaterialsByUserIdAsync(new FIIT_folder.Domain.Value_Object.UserId(request.UserId.Value), cancellationToken);
            isFavorite = favorites.Any(f => f.MaterialId.Value == request.Id);
        }

        var (likes, dislikes) = await _ratingRepository.GetRatingCountsAsync(material.Id.Value, cancellationToken);
        string? currentUserRating = null;
        if (request.UserId.HasValue)
        {
            var rating = await _ratingRepository.GetByUserAndMaterialAsync(
                new FIIT_folder.Domain.Value_Object.UserId(request.UserId.Value),
                material.Id);
            currentUserRating = rating?.Rating.ToString();
        }

        return new MaterialDto
        {
            Id = material.Id.Value,
            SubjectId = material.SubjectId.Value,
            Name = material.Name.Value,
            Year = material.Year.Value,
            Semester = material.Semester.Value,
            Description = material.Description, // Assuming this property is added to StudyMaterial
            AuthorName = authorName,
            IsFavorite = isFavorite,
            MaterialType = material.MaterialType.ToString(),
            Size = material.Size.Size,
            FilePath = material.FilePath.Value,
            UploadedAt = material.UploadedAt,
            LikesCount = likes,
            DislikesCount = dislikes,
            CurrentUserRating = currentUserRating
        };
    }
}
