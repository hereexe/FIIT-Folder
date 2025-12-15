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
        var filter = Builders<User>.Filter.Eq("Login.Value", login);
        return await _usersCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);
    }
}