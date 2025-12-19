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
        Console.WriteLine($"[LOGIN] Attempting login for: '{request.Username}'");
        
        var user = await _userRepository.GetByLoginAsync(request.Username, cancellationToken);

        if (user == null)
        {
            Console.WriteLine($"[LOGIN] User NOT FOUND for login: '{request.Username}'");
            throw new Exception("Неверный логин или пароль");
        }
        
        Console.WriteLine($"[LOGIN] User found: Id={user.Id.Value}, Login='{user.Login.Value}'");
        
        var passwordValid = _passwordHasher.Verify(request.Password, user.PasswordHash.Value);
        Console.WriteLine($"[LOGIN] Password verification result: {passwordValid}");
        
        if (!passwordValid)
        {
            throw new Exception("Неверный логин или пароль");
        }

        var token = _jwtProvider.GenerateToken(user);
        Console.WriteLine($"[LOGIN] Token generated successfully");
        return token;
    }
}