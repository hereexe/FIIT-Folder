namespace FIIT_folder.Domain.Entities;

public class User
{
    public Guid Id {get; private set;}
    public string Login { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public User(string login, string passwordHash, string role = "Student", DateTime createdAt = default)
    {
        Id = Guid.NewGuid();
        
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login cannot be empty");
        Login = login;
        
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password cannot be empty");
        PasswordHash = passwordHash;
        
        Role = role;
        CreatedAt = createdAt == default ? DateTime.UtcNow : createdAt;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    private User() {}
}