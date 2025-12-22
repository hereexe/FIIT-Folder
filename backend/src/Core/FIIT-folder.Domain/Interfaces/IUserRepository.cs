using FIIT_folder.Domain.Entities;

namespace FIIT_folder.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByLogin(string login, CancellationToken cancellationToken = default);
    Task<User?> GetById(Guid id, CancellationToken cancellationToken = default);
    Task CreateUser(User user, CancellationToken cancellationToken = default);
}