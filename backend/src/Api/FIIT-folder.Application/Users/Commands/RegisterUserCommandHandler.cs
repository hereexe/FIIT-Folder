using FIIT_folder.Application.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;
using MediatR;

namespace FIIT_folder.Application.Users.Commands;

public class RegisterUserCommandHandler: IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByLogin(request.Username, cancellationToken);
        if (existingUser != null)
        {
            throw new Exception("Пользователь с таким логином уже существует.");
        }
        
        var login = Login.Create(request.Username);
        var passwordHash = PasswordHash.Create(_passwordHasher.Hash(request.Password));
        var userId = UserId.New();
        
        var user = new User(userId, login, passwordHash, UserRole.Student);
        
        await _userRepository.CreateUser(user, cancellationToken);
        return userId.Value;
    }

}