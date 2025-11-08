using MediatR;
using Microsoft.AspNetCore.Mvc;



namespace FIIT_folder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController
{
    private readonly IMediator _mediator;

    public SubjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
    }
}