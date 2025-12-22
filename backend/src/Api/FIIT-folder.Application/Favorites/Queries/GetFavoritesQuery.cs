using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using MediatR;

namespace FIIT_folder.Application.Favorites.Queries;

public record GetFavoritesQuery(Guid UserId) : IRequest<List<FavoriteMaterialDto>>;

public class GetFavoritesHandler : IRequestHandler<GetFavoritesQuery, List<FavoriteMaterialDto>>
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMaterialMongoDB _materialRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMaterialRatingRepository _ratingRepository;

    public GetFavoritesHandler(
        IFavoriteRepository favoriteRepository, 
        IMaterialMongoDB materialRepository,
        IUserRepository userRepository,
        IMaterialRatingRepository ratingRepository)
    {
        _favoriteRepository = favoriteRepository;
        _materialRepository = materialRepository;
        _userRepository = userRepository;
        _ratingRepository = ratingRepository;
    }

    public async Task<List<FavoriteMaterialDto>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var favMaterials = await _favoriteRepository.GetMaterialsByUserIdAsync(userId, cancellationToken);

        var response = new List<FavoriteMaterialDto>();

        foreach (var favMat in favMaterials)
        {
            string materialName = "Unknown Material";
            var matDto = new FavoriteMaterialDto
            {
                Id = favMat.Id, // Favorite ID
                MaterialId = favMat.MaterialId.Value, // Material ID
                AddedAt = favMat.AddedAt
            };

            try 
            {
                var mat = await _materialRepository.GetByIdStudyMaterial(favMat.MaterialId.Value);
                if (mat != null) 
                {
                    materialName = mat.Name.Value;
                    matDto.Name = materialName;
                    matDto.FilePath = mat.FilePath.Value;
                    matDto.MaterialType = mat.MaterialType.ToString();
                    matDto.Size = mat.Size.Size;
                    matDto.Year = mat.Year.Value;
                    matDto.Semester = mat.Semester.Value;
                    matDto.Description = mat.Description;
                    matDto.SubjectId = mat.SubjectId.Value;

                    matDto.SizeFormatted = FormatSize(mat.Size.Size);
                    // Since we are in Application layer, we don't have HTTP Request. 
                    // Frontend will handle the base URL or we can pass it if needed.
                    matDto.DownloadUrl = $"/api/materials/{mat.Id.Value}/download";

                    var user = await _userRepository.GetByIdAsync(mat.UserId.Value, cancellationToken);
                    if (user != null)
                    {
                        matDto.AuthorName = user.Login.Value;
                    }

                    var (likes, dislikes) = await _ratingRepository.GetRatingCountsAsync(mat.Id.Value, cancellationToken);
                    matDto.LikesCount = likes;
                    matDto.DislikesCount = dislikes;
                    
                    var rating = await _ratingRepository.GetByUserAndMaterialAsync(
                        new FIIT_folder.Domain.Value_Object.UserId(request.UserId),
                        mat.Id);
                    matDto.CurrentUserRating = rating?.Rating.ToString();
                }
                else
                {
                    matDto.Name = materialName;
                }
            }
            catch 
            {
                matDto.Name = materialName;
            }

            response.Add(matDto);
        }

        return response;
    }

    private static string FormatSize(long bytes)
    {
        return bytes switch
        {
            >= 1024 * 1024 => $"{bytes / 1024.0 / 1024.0:F2} MB",
            >= 1024 => $"{bytes / 1024.0:F2} KB",
            _ => $"{bytes} B"
        };
    }
}
