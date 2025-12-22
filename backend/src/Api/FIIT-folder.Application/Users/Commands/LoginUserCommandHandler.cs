using MediatR;
using FIIT_folder.Application.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;

namespace FIIT_folder.Application.Users.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByLogin(request.Username, cancellationToken);

        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash.Value))
        {
            throw new Exception("Неверный логин или пароль");
        }

        var token = _jwtProvider.GenerateToken(user);
        return token;
    }
}