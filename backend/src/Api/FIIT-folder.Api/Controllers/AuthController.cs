using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using FIIT_folder.Application.Users.Commands;
using Microsoft.AspNetCore.Identity.Data;
using RegisterRequest = FIIT_folder.Api.Models.RegisterRequest;
using LoginRequest = FIIT_folder.Api.Models.LoginRequest;

namespace FIIT_folder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Username, request.Password);
        var userId = await _mediator.Send(command);
        
        return Ok(new {userId = userId});
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginUserCommand(request.Username, request.Password);
        var token = await _mediator.Send(command);
        
        return Ok(new {token = token});
    }


}