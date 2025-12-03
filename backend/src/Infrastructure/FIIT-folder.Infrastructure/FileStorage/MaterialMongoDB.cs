using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.FileStorage;

public class MaterialMongoDB : IMaterialMongoDB
{
    private readonly IMongoCollection<BsonDocument> _collection;
    public MaterialMongoDB(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"];
        var name = configuration["MongoDbSettings:DatabaseName"];
            
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(name);
        _collection = database.GetCollection<BsonDocument>("materials");
    }
    
    public async Task<StudyMaterial> CreateMaterial(StudyMaterial material)
    {
        try
        {
            Console.WriteLine("Сохраняю материал");
            
            var document = new BsonDocument
            {
                { "materialId", material.Id.Value.ToString() },
                { "subjectId", material.SubjectId.Value.ToString() },
                { "userId", material.UserId.Value.ToString() },
                { "name", material.Name.Value },
                { "year", material.Year.Value },
                { "size", material.Size.ToString() },
                { "type", material.MaterialType.ToString() },
                { "filePath", material.FilePath.Value },
                { "uploadedAt", material.UploadedAt }
            };
            
            await _collection.InsertOneAsync(document);
            return material;
        }
        catch (Exception ex)
        {
            throw new Exception("Ошибка в сохранении файла");
        }
    }
    
    public Task<StudyMaterial> GetByIdMaterial(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMaterial(string id)
    {
        throw new NotImplementedException();
    }
}