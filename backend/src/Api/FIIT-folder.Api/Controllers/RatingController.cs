using System.Security.Claims;
using FIIT_folder.Api.Models;
using FIIT_folder.Application.Ratings.Commands;
using FIIT_folder.Application.Ratings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIIT_folder.Api.Controllers;

[ApiController]
[Route("api/materials")]
public class RatingController : ControllerBase
{
    private readonly IMediator _mediator;

    public RatingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{materialId}/rate")]
    [Authorize]
    public async Task<IActionResult> Rate(Guid materialId, [FromBody] RateRequest request)
    {
        var userId = GetUserIdFromToken();
        var command = new RateMaterialCommand(materialId, userId, request.RatingType);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{materialId}/rating")]
    public async Task<IActionResult> GetRating(Guid materialId)
    {
        var query = new GetRatingQuery(materialId);
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