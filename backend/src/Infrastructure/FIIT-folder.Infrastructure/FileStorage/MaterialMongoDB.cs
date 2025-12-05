using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
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
        var databaseName = configuration["MongoDbSettings:DatabaseName"];

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<BsonDocument>("materials");
    }

    public async Task<StudyMaterial> Create(StudyMaterial material)
    {
        try
        {
            var document = new BsonDocument
            {
                { "materialId", material.Id.Value.ToString() },
                { "subjectId", material.SubjectId.Value.ToString() },
                { "userId", material.UserId.Value.ToString() },
                { "name", material.Name.Value },
                { "year", material.Year.Value },
                { "materialType", material.MaterialType.ToString() },
                { "size", material.Size.Size },
                { "filePath", material.FilePath.Value },
                { "uploadedAt", material.UploadedAt }
            };

            await _collection.InsertOneAsync(document);
            Console.WriteLine($"Материал '{material.Name.Value}' сохранён в MongoDB");
            return material;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при сохранении материала: {ex.Message}", ex);
        }
    }

    public async Task<StudyMaterial?> GetByIdMaterial(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("materialId", id.ToString());
            var document = await _collection.Find(filter).FirstOrDefaultAsync();

            if (document == null)
                return null;

            return MapToStudyMaterial(document);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении материала: {ex.Message}", ex);
        }
    }

    public async Task<List<StudyMaterial>> GetBySubjectId(Guid subjectId)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("subjectId", subjectId.ToString());
            var documents = await _collection.Find(filter).ToListAsync();
            return documents.Select(MapToStudyMaterial).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении материалов по предмету: {ex.Message}", ex);
        }
    }

    public async Task<List<StudyMaterial>> GetAll()
    {
        try
        {
            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            return documents.Select(MapToStudyMaterial).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении списка материалов: {ex.Message}", ex);
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("materialId", id.ToString());
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при удалении материала: {ex.Message}", ex);
        }
    }

    //TODO: сделать норм мапинг потом 
    private static StudyMaterial MapToStudyMaterial(BsonDocument document)
    {
        var name = new MaterialName(document["name"].AsString);
        var subjectId = new SubjectId(Guid.Parse(document["subjectId"].AsString));
        var userId = new UserId(Guid.Parse(document["userId"].AsString));
        var year = new StudyYear(document["year"].AsInt32);
        var size = new MaterialSize(document["size"].AsInt64);
        var materialType = Enum.Parse<MaterialType>(document["materialType"].AsString, ignoreCase: true);
        var filePath = new ResourceLocation(document["filePath"].AsString);

        var material = new StudyMaterial(name, subjectId, userId, year, size, materialType, filePath);
        
        var idProperty = typeof(StudyMaterial).GetProperty("Id");
        var uploadedAtProperty = typeof(StudyMaterial).GetProperty("UploadedAt");
        
        idProperty?.SetValue(material, new StudyMaterialId(Guid.Parse(document["materialId"].AsString)));
        uploadedAtProperty?.SetValue(material, document["uploadedAt"].ToUniversalTime());

        return material;
    }
}
