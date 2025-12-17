using System.Security.Claims;
using FIIT_folder.Application.Favorites.Commands;
using FIIT_folder.Application.Favorites.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIIT_folder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("materials")]
    public async Task<IActionResult> AddMaterial([FromBody] AddMaterialRequest request)
    {
        var userId = GetUserIdFromToken();
        var command = new AddFavoriteMaterialCommand(userId, request.MaterialId);
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = GetUserIdFromToken(); // Used for checking, though command currently only takes ItemId, see logic note.
        // Ideally we should pass userId to command to verify ownership.
        // For now, consistent with existing command structure, but I won't pass userId if command doesn't take it and check it.
        // But wait, the user wants "auto userId from token".
        // The previous code was: public async Task<IActionResult> Remove(Guid id, [FromQuery] Guid userId)
        // And command: new RemoveFavoriteCommand(userId, id);
        // So command DOES take userId.
        
        var command = new RemoveFavoriteCommand(userId, id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetFavorites()
    {
        var userId = GetUserIdFromToken();
        var query = new GetFavoritesQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    private Guid GetUserIdFromToken()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (claim == null || !Guid.TryParse(claim.Value, out var userId))
            throw new UnauthorizedAccessException("Неверный токен");
        return userId;
    }
}

public record AddMaterialRequest(Guid MaterialId);
