using System.Security.Claims;
using FIIT_folder.Api.Models;
using FIIT_folder.Application.Materials.Commands;
using FIIT_folder.Application.Materials.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIIT_folder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MaterialResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySubject([FromQuery] GetMaterialsRequest request)
    {
        var userId = GetUserIdOrNull();
        var materials = await _mediator.Send(new GetMaterialsBySubjectQuery(request.SubjectId!.Value, userId, request.Semester));

        var result = materials.Select(m => new MaterialResponse
        {
            Id = m.Id,
            SubjectId = m.SubjectId,
            Name = m.Name,
            Year = m.Year,
            Semester = m.Semester,
            Description = m.Description,
            AuthorName = m.AuthorName,
            IsFavorite = m.IsFavorite,
            MaterialType = m.MaterialType,
            Size = FormatSize(m.Size),
            UploadedAt = m.UploadedAt
        }).AsEnumerable();

        if (!string.IsNullOrEmpty(request.MaterialType))
            result = result.Where(m => m.MaterialType.Equals(request.MaterialType, StringComparison.OrdinalIgnoreCase));

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(MaterialResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] UploadMaterialRequest request)
    {
        await using var stream = request.File.OpenReadStream();
        var userId = GetUserId();

        var command = new UploadMaterialCommand(
            request.SubjectId,
            userId,
            request.File.FileName,
            request.Year,
            request.Semester,
            request.Description,
            request.MaterialType,
            request.File.Length,
            request.File.ContentType,
            stream);

        var result = await _mediator.Send(command);

        var response = new MaterialResponse
        {
            Id = result.Id,
            SubjectId = result.SubjectId,
            Name = result.Name,
            Year = result.Year,
            Semester = result.Semester,
            Description = result.Description,
            MaterialType = result.MaterialType,
            Size = FormatSize(result.Size),
            UploadedAt = result.UploadedAt,
            // AuthorName and IsFavorite might be empty/false initially or we can fetch them if needed. 
            // Since it's just uploaded, Author is current user, IsFavorite is false.
            AuthorName = User.Identity?.Name ?? "Me", // Or fetch user login if available in token properly
            IsFavorite = false
        };

        return Created($"/api/materials/{response.Id}", response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MaterialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetUserIdOrNull();
        var material = await _mediator.Send(new GetMaterialByIdQuery(id, userId));
        
        if (material == null)
            return NotFound();

        var response = new MaterialResponse
        {
            Id = material.Id,
            SubjectId = material.SubjectId,
            Name = material.Name,
            Year = material.Year,
            Semester = material.Semester,
            Description = material.Description,
            AuthorName = material.AuthorName,
            IsFavorite = material.IsFavorite,
            MaterialType = material.MaterialType,
            Size = FormatSize(material.Size),
            UploadedAt = material.UploadedAt
        };

        return Ok(response);
    }

    [HttpGet("{id}/download")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(Guid id)
    {
        var result = await _mediator.Send(new DownloadMaterialQuery(id));
        return File(result.FileStream, result.ContentType, result.FileName);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteMaterialCommand(id));
        return NoContent();
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

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (claim == null || !Guid.TryParse(claim.Value, out var userId))
            throw new UnauthorizedAccessException("Неверный токен");
        return userId;
    }

    private Guid? GetUserIdOrNull()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
        if (claim != null && Guid.TryParse(claim.Value, out var userId))
            return userId;
        return null;
    }
}