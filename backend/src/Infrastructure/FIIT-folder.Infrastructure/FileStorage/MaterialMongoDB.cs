using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.FileStorage;

public class MaterialMongoDB : IMaterialMongoDB
{
    public MaterialMongoDB(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"];
        var name = configuration["MongoDbSettings:DatabaseName"];
            
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(name);
    }
    
    public async Task<Material> CreateMaterial(Material material)
    {
        try
        {
            Console.WriteLine("Сохраняю материал");
            
            var document = new BsonDocument //пока обычный словарик чтобы просто проверить
            {
                { "materialId", material.Id.ToString() },
                { "name", material.Name },
                { "path", material.Path },
                { "size", material.Size },
                { "type", material.Type },
                { "date", DateTime.UtcNow },
            };
            
            return material;
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка в сохранении файла");
        }
    }
    
    public Task<Material> GetByIdMaterial(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMaterial(string id)
    {
        throw new NotImplementedException();
    }
}