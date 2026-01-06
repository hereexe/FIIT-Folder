using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.Persistence;

public class UserMongoDB : IUserRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserMongoDB(string connectionString, string databaseName)
    {
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);
        _usersCollection = mongoDatabase.GetCollection<User>("users");
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken =
        default)
    {
        var normalizedLogin = login.Trim().ToLowerInvariant();
        var filter = Builders<User>.Filter.Eq(u => u.Login.Value, normalizedLogin);
        return await _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id.Value, id);
        return await _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id.Value, user.Id.Value);
        await _usersCollection.ReplaceOneAsync(filter, user, cancellationToken: cancellationToken);
    }
}