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
        var computedHash = Hash(password);
        Console.WriteLine($"[PasswordHasher] Computed hash: '{computedHash}'");
        Console.WriteLine($"[PasswordHasher] Stored hash:   '{hashedPassword}'");
        Console.WriteLine($"[PasswordHasher] Match: {computedHash == hashedPassword}");
        return computedHash == hashedPassword;
    }
}
