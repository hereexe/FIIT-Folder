using FIIT_folder.Api.Models;
using FIIT_folder.Domain.Entities;
using MediatR;
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
    public async Task<IActionResult> GetBySubject([FromQuery] Guid? subjectId, [FromQuery] string? materialType)
    {
        // TODO: заменить на реальное получение из БД
        var mockMaterials = new List<MaterialResponse>
        {
            new()
            {
                Id = Guid.NewGuid(),
                SubjectId = subjectId ?? Guid.NewGuid(),
                Name = "Экзамен_2024.pdf",
                Year = 2024,
                MaterialType = "Exam",
                Size = "2.5 MB",
                UploadedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                SubjectId = subjectId ?? Guid.NewGuid(),
                Name = "Коллоквиум_1.docx",
                Year = 2024,
                MaterialType = "Colloquium",
                Size = "1.2 MB",
                UploadedAt = DateTime.UtcNow
            }
        };

        var result = mockMaterials.AsEnumerable();
        
        if (!string.IsNullOrEmpty(materialType))
            result = result.Where(m => m.MaterialType.Equals(materialType, StringComparison.OrdinalIgnoreCase));

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MaterialResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] UploadMaterialRequest request)
    {
        // Валидация выполняется в ValidationFilter через FluentValidation
        var materialType = Enum.Parse<MaterialType>(request.MaterialType, ignoreCase: true);

        // TODO: проверить что materialType разрешён для данного Subject
        // TODO: заменить на реальное сохранение через сервис/команду

        var response = new MaterialResponse
        {
            Id = Guid.NewGuid(),
            SubjectId = request.SubjectId,
            Name = request.File!.FileName,
            Year = request.Year,
            MaterialType = materialType.ToString(),
            Size = $"{request.File.Length / 1024.0 / 1024.0:F2} MB",
            UploadedAt = DateTime.UtcNow
        };

        return Created("", response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MaterialResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        // TODO: заменить на реальное получение из БД
        var mockMaterial = new MaterialResponse
        {
            Id = id,
            SubjectId = Guid.NewGuid(),
            Name = "Экзамен_2024.pdf",
            Year = 2024,
            MaterialType = "Exam",
            Size = "2.5 MB",
            UploadedAt = DateTime.UtcNow
        };

        return Ok(mockMaterial);
    }

    [HttpGet("{id}/download-link")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDownloadLink(Guid id)
    {
        // TODO: заменить на реальную генерацию ссылки из файлового хранилища
        var mockUrl = $"https://storage.example.com/materials/{id}";
        return Ok(new { downloadUrl = mockUrl, expiresAt = DateTime.UtcNow.AddHours(1) });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        // TODO: заменить на реальное удаление через сервис/команду
        return NoContent();
    }
}