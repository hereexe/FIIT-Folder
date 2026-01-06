using FIIT_folder.Domain.Entities;

namespace FIIT_folder.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}