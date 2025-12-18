using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FIIT_folder.Application.Interfaces;
using FIIT_folder.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FIIT_folder.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    public string GenerateToken(User user)
    {
        var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                        ?? throw new InvalidOperationException("JWT_SECRET_KEY is not configured");

        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                     ?? throw new InvalidOperationException("JWT_ISSUER is not configured");

        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                       ?? throw new InvalidOperationException("JWT_AUDIENCE is not configured");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Login.Value),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(45),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}