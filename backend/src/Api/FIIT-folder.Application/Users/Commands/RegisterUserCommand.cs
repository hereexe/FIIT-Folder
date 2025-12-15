using MediatR;

namespace FIIT_folder.Application.Users.Commands;

public record RegisterUserCommand(string Username, string Password) : IRequest<Guid>;  
