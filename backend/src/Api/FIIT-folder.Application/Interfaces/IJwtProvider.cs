using FIIT_folder.Domain.Entities;

namespace FIIT_folder.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}