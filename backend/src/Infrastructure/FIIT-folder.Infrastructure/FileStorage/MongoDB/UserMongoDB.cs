using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.Persistence;

public class UserMongoDB : IUserRepository
{
    private readonly IMongoCollection<User> Collection;

    public UserMongoDB(string connectionString, string databaseName)
    {
        var mongoClient = new MongoClient(connectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseName);
        Collection = mongoDatabase.GetCollection<User>("users");
    }

    public async Task<User?> GetByLogin(string login, CancellationToken cancellationToken =
        default)
    {
        var normalizedLogin = login.Trim().ToLowerInvariant();
        var filter = Builders<User>.Filter.Eq(u => u.Login.Value, normalizedLogin);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateUser(User user, CancellationToken cancellationToken = default)
    {
        await Collection.InsertOneAsync(user, cancellationToken: cancellationToken);
    }

    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Id.Value, id);
        return await Collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}