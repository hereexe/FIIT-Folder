using FIIT_folder.Domain.Interfaces;
using MediatR;

namespace FIIT_folder.Application.Favorites.Commands;

public record RemoveFavoriteCommand(Guid UserId, Guid ItemId) : IRequest;

public class RemoveFavoriteHandler : IRequestHandler<RemoveFavoriteCommand>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public RemoveFavoriteHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
    {
        var userId = new FIIT_folder.Domain.Value_Object.UserId(request.UserId);
        await _favoriteRepository.RemoveMaterialAsync(userId, request.ItemId, cancellationToken);
    }
}
