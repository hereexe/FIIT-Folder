using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Domain.Entities;

public class User
{
    public UserId Id {get; private set;}
    public Login Login { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(UserId id, Login login, PasswordHash passwordHash, UserRole role)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(PasswordHash newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    private User() {}
}