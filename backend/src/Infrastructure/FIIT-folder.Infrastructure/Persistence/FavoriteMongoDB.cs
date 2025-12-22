using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.Persistence;

public class FavoriteMongoDB : IFavoriteRepository
{
    private readonly IMongoCollection<FavoriteMaterial> Materials;
    public FavoriteMongoDB(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        Materials = database.GetCollection<FavoriteMaterial>("FavoriteMaterials");
    }

    public async Task CreateMaterial(FavoriteMaterial material, CancellationToken cancellationToken)
    {
        await Materials.InsertOneAsync(material, cancellationToken);
    }

    public async Task<List<FavoriteMaterial>> GetMaterialsByUserId(UserId userId, CancellationToken cancellationToken)
    {
        return await Materials.Find(m => m.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task DeleteMaterialAsync(Guid id, CancellationToken cancellationToken)
    {
        await Materials.DeleteOneAsync(m => m.Id == id, cancellationToken);
    }
}
