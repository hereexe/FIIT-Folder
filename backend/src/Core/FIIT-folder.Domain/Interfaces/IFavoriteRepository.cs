using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Domain.Interfaces;

public interface IFavoriteRepository
{
    Task AddMaterialAsync(FavoriteMaterial material, CancellationToken cancellationToken);
    Task<List<FavoriteMaterial>> GetMaterialsByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task RemoveMaterialAsync(Guid id, CancellationToken cancellationToken);
}
