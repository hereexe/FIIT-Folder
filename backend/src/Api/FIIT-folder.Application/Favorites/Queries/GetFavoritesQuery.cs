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

    public GetFavoritesHandler(
        IFavoriteRepository favoriteRepository, 
        IMaterialMongoDB materialRepository,
        IUserRepository userRepository)
    {
        _favoriteRepository = favoriteRepository;
        _materialRepository = materialRepository;
        _userRepository = userRepository;
    }

    public async Task<List<FavoriteMaterialDto>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var favMaterials = await _favoriteRepository.GetMaterialsByUserId(userId, cancellationToken);

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
                var mat = await _materialRepository.GetByIdMaterial(favMat.MaterialId.Value);
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

                    var user = await _userRepository.GetByIdAsync(mat.UserId.Value, cancellationToken);
                    if (user != null)
                    {
                        matDto.AuthorName = user.Login.Value;
                    }
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
