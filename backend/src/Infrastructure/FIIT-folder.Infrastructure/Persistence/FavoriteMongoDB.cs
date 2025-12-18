using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.Persistence;

public class FavoriteMongoDB : IFavoriteRepository
{
    private readonly IMongoCollection<FavoriteMaterial> _materials;

    public FavoriteMongoDB(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _materials = database.GetCollection<FavoriteMaterial>("FavoriteMaterials");
    }

    public async Task AddMaterialAsync(FavoriteMaterial material, CancellationToken cancellationToken)
    {
        await _materials.InsertOneAsync(material, cancellationToken: cancellationToken);
    }

    public async Task<List<FavoriteMaterial>> GetMaterialsByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await _materials.Find(m => m.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task RemoveMaterialAsync(Guid id, CancellationToken cancellationToken)
    {
        await _materials.DeleteOneAsync(m => m.Id == id, cancellationToken);
    }
}
