using MediatR;

namespace FIIT_folder.Application.Users.Commands;

public record LoginUserCommand(string Username, string Password) : IRequest<string>; //jwt надо бы 