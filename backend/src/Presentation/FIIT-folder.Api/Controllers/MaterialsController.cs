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
    [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySubject([FromQuery] Guid? subjectId)
    {
        // TODO: Заглушка пока что 
        var mockMaterials = new[]
        {
            new { Id = Guid.NewGuid(), Name = "Экзамен 1.pdf", Type = "Экзамен" },
            new { Id = Guid.NewGuid(), Name = "Коллоквиум 2.docx", Type = "Коллоквиум" }
        };
        return Ok(mockMaterials.Where(m => subjectId.HasValue));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] string subject, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Файл не выбран или пуст.");
        }
        
        var newMaterialId = Guid.NewGuid();
        return CreatedAtAction(nameof(GetBySubject), new { subject = newMaterialId }, new { Id = newMaterialId });
    }

    [HttpGet("{id}/download-link")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDownloadLink(Guid id)
    {
        //для примера пусть так будет, а так нужно будет переделать для Файлового хранилища
        var mockUrl = $"http://localhost:5000/api/materials/upload";
        return Ok(new { downloadUrl = mockUrl });
    }

}