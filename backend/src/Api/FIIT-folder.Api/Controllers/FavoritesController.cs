using FIIT_folder.Application.Favorites.Commands;
using FIIT_folder.Application.Favorites.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FIIT_folder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var command = new AddFavoriteMaterialCommand(request.UserId, request.MaterialId);
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id, [FromQuery] Guid userId)
    {
        var command = new RemoveFavoriteCommand(userId, id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetFavorites([FromQuery] Guid userId)
    {
        var query = new GetFavoritesQuery(userId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

public record AddMaterialRequest(Guid UserId, Guid MaterialId);
