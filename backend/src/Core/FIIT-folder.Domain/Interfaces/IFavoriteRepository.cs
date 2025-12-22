using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Domain.Interfaces;

public interface IFavoriteRepository
{
    Task CreateMaterial(FavoriteMaterial material, CancellationToken cancellationToken);
    Task<List<FavoriteMaterial>> GetMaterialsByUserId(UserId userId, CancellationToken cancellationToken);
    Task DeleteMaterialAsync(Guid id, CancellationToken cancellationToken);
}
