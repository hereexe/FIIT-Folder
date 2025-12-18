using MediatR;
using Microsoft.AspNetCore.Mvc;
using FIIT_folder.Api.Models;
using FIIT_folder.Application.Subjects.Commands;
using FIIT_folder.Application.Subjects.Queries;

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
        var command = new CreateSubjectCommand(request.Name, request.Semester, request.MaterialTypes);
        var result = await _mediator.Send(command);

        var response = new SubjectResponse
        {
            Id = result.Id,
            Name = result.Name,
            Semester = result.Semester,
            MaterialTypes = result.MaterialTypes.Select(t => new MaterialTypeResponse
            {
                Value = t.Value,
                DisplayName = t.DisplayName
            }).ToList()
        };

        return Created($"/api/subjects/{response.Id}", response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SubjectResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var subjects = await _mediator.Send(new GetAllSubjectsQuery());

        var response = subjects.Select(s => new SubjectResponse
        {
            Id = s.Id,
            Name = s.Name,
            Semester = s.Semester,
            MaterialTypes = s.MaterialTypes.Select(t => new MaterialTypeResponse
            {
                Value = t.Value,
                DisplayName = t.DisplayName
            }).ToList()
        });

        return Ok(response);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SubjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var subject = await _mediator.Send(new GetSubjectByIdQuery(id));

        var response = new SubjectResponse
        {
            Id = subject.Id,
            Name = subject.Name,
            Semester = subject.Semester,
            MaterialTypes = subject.MaterialTypes.Select(t => new MaterialTypeResponse
            {
                Value = t.Value,
                DisplayName = t.DisplayName
            }).ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Получить предмет с материалами, сгруппированными по типам
    /// </summary>
    [HttpGet("{id}/materials")]
    [ProducesResponseType(typeof(SubjectWithMaterialsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithMaterials(Guid id)
    {
        var result = await _mediator.Send(new GetSubjectWithMaterialsQuery(id));

        var response = new SubjectWithMaterialsResponse
        {
            Id = result.Id,
            Name = result.Name,
            Content = result.MaterialGroups.Select(g => new MaterialGroupResponse
            {
                ExamType = g.ExamType,
                ExamNames = g.ExamNames
            }).ToList()
        };

        return Ok(response);
    }
    
    [HttpGet("{id}/material-types")]
    [ProducesResponseType(typeof(IEnumerable<MaterialTypeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMaterialTypes(Guid id)
    {
        var subject = await _mediator.Send(new GetSubjectByIdQuery(id));
        var response = subject.MaterialTypes.Select(t => new MaterialTypeResponse
        {
            Value = t.Value,
            DisplayName = t.DisplayName
        });

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteSubjectCommand(id));
        return NoContent();
    }
}
