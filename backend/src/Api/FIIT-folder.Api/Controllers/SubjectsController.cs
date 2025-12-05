using MediatR;
using Microsoft.AspNetCore.Mvc;
using FIIT_folder.Api.Models;
using FIIT_folder.Domain.Entities;

namespace FIIT_folder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(SubjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSubjectRequest request)
    {
        // Валидация выполняется в ValidationFilter через FluentValidation
        var materialTypes = request.MaterialTypes
            .Select(t => Enum.Parse<MaterialType>(t, ignoreCase: true))
            .ToList();

        // TODO: заменить на реальное создание через сервис/команду
        var response = new SubjectResponse
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Semester = request.Semester,
            MaterialTypes = materialTypes.Select(t => t.ToString()).ToList()
        };

        return Created("", response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SubjectResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        // TODO: заменить на реальное получение из БД
        var mockSubjects = new List<SubjectResponse>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Математический анализ",
                Semester = 1,
                MaterialTypes = new List<string> { "Exam", "Colloquium", "ControlWork" }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ОРГ",
                Semester = 2,
                MaterialTypes = new List<string> { "Pass" }
            }
        };

        return Ok(mockSubjects);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SubjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        // TODO: заменить на реальное получение из БД
        var mockSubject = new SubjectResponse
        {
            Id = id,
            Name = "Математический анализ",
            Semester = 1,
            MaterialTypes = new List<string> { "Exam", "Colloquium", "ControlWork" }
        };

        return Ok(mockSubject);
    }
    
    [HttpGet("{id}/material-types")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMaterialTypes(Guid id)
    {
        // TODO: заменить на реальное получение из БД
        var materialTypes = new List<string> { "Exam", "Colloquium", "ControlWork" };

        return Ok(materialTypes);
    }
}
