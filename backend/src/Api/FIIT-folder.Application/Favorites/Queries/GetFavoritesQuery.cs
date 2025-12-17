using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using MediatR;

namespace FIIT_folder.Application.Favorites.Queries;

public record GetFavoritesQuery(Guid UserId) : IRequest<List<FavoriteMaterialDto>>;

public class GetFavoritesHandler : IRequestHandler<GetFavoritesQuery, List<FavoriteMaterialDto>>
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IMaterialMongoDB _materialRepository;

    public GetFavoritesHandler(IFavoriteRepository favoriteRepository, IMaterialMongoDB materialRepository)
    {
        _favoriteRepository = favoriteRepository;
        _materialRepository = materialRepository;
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
                    matDto.SubjectId = mat.SubjectId.Value;
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
}
