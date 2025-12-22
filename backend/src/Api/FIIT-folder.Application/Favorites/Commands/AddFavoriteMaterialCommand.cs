using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using MediatR;

namespace FIIT_folder.Application.Favorites.Commands;

public record AddFavoriteMaterialCommand(Guid UserId, Guid MaterialId) : IRequest<Guid>;

public class AddFavoriteMaterialHandler : IRequestHandler<AddFavoriteMaterialCommand, Guid>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public AddFavoriteMaterialHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<Guid> Handle(AddFavoriteMaterialCommand request, CancellationToken cancellationToken)
    {
        var userIdObj = new UserId(request.UserId);
        var matIdObj = new StudyMaterialId(request.MaterialId);

        var existing = await _favoriteRepository.GetMaterialsByUserIdAsync(userIdObj, cancellationToken);
        var existingFav = existing.FirstOrDefault(m => m.MaterialId == matIdObj);
        
        if (existingFav != null)
        {
            return existingFav.Id;
        }

        var favorite = new FavoriteMaterial(userIdObj, matIdObj);
            
        await _favoriteRepository.AddMaterialAsync(favorite, cancellationToken);
        return favorite.Id;
    }
}
