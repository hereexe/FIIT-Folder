using System.Security.Cryptography;
using System.Text;
using FIIT_folder.Application.Interfaces;

namespace FIIT_folder.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public bool Verify(string password, string hashedPassword)
    {
        return Hash(password) == hashedPassword;
    }
}
