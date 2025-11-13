namespace FIIT_folder.Domain.Entities;

public class User
{
    public Guid Id {get; private set;}
    public string Login { get; private set; }
    public string PasswordHash { get; set; }
    public string Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User(string login, string passwordHash, string role = "Student", DateTime createdAt = DateTime.UtcNow)
    {
        Id = Guid.NewGuid();
        
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login cannot be empty");
        Login = login;
        
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password cannot be empty");
        PasswordHash = passwordHash;
        
        Role = role;
        CreatedAt = createdAt;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }

    private User() {}
}