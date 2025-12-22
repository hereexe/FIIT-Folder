using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.FileStorage;

public class MaterialMongoDB : IMaterialMongoDB
{
    private readonly IMongoCollection<BsonDocument> Collection;
    
    public MaterialMongoDB(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        Collection = database.GetCollection<BsonDocument>("Material");
        Console.WriteLine($"MongoDB подключен: {databaseName}");
        
        //CreateIndexes(); //мб понадабятся еще для админа
    }
    
    public MaterialMongoDB(IMongoCollection<BsonDocument> collection)
    {
        Collection = collection;
        //CreateIndexes();
    }
    
    //public async Task<Material> CreateMaterial(Material material)
    // private void CreateIndexes()
    // {
    //     try
    //     {
    //         var idIndex = Builders<Material>.IndexKeys.Ascending(m => m.Id);
    //         Materials.Indexes.CreateOne(new CreateIndexModel<Material>
    //             (idIndex, new CreateIndexOptions { Unique = true })
    //         );
    //         Console.WriteLine("Создан уникальный индекс по Id");
    //         
    //         var subjectIdIndex = Builders<Material>.IndexKeys.Ascending(m => m.SubjectId);
    //         Materials.Indexes.CreateOne(
    //             new CreateIndexModel<Material>(subjectIdIndex)
    //         );
    //         Console.WriteLine("Создан индекс по SubjectId");
    //         
    //         var userIdIndex = Builders<Material>.IndexKeys.Ascending(m => m.UserId);
    //         Materials.Indexes.CreateOne(
    //             new CreateIndexModel<Material>(userIdIndex)
    //         );
    //         Console.WriteLine("Создан индекс по UserId");
    //     }
    //     
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Ошибка создания индексов");
    //         Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
    //         
    //     }
    // }

    public async Task<Material> CreateMaterial(Material material)
    {
        try
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material), "Material не должен быть null");
            var bsonDocument = MapToBsonDocument(material);
            
            await Collection.InsertOneAsync(bsonDocument);
            Console.WriteLine($"Материал сохранен в MongoDB!");
            
            return material;
        }
        catch (Exception ex)
        {
            // Console.WriteLine(ex.Message);
            // Console.WriteLine(ex.GetType().Name);
            throw new Exception($"Ошибка сохранении материала: {ex.Message}", ex);
        }
    }

    public async Task<Material?> GetByIdMaterial(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("materialId", id.ToString());
            var document = await Collection.Find(filter).FirstOrDefaultAsync();
            if (document == null)
                return null;

            return MapToMaterial(document);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении материала: {ex.Message}", ex);
        }
    }
    
    public async Task<Material?> GetByNameMaterial(string materialName)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("name", materialName);
            var document = await Collection.Find(filter).FirstOrDefaultAsync();
            if (document == null)
                return null;
            return MapToMaterial(document);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении материала: {ex.Message}", ex);
        }
    }

    public async Task<List<Material>> GetBySubjectId(Guid subjectId)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("subjectId", subjectId.ToString());
            var documents = await Collection.Find(filter).ToListAsync();
            return documents.Select(MapToMaterial).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении материалов по subjectId: {ex.Message}");
            return new List<Material>();
        }
    }

    public Task<Material> UpdateMaterial(Material material) //пока не нужен
    {
        try
        {
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении материалов по предмету: {ex.Message}", ex);
        }
    }

    public async Task<List<Material>> GetAll() //пока не нужен
    {
        return null;
    }

    public async Task<bool> DeleteMaterial(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("materialId", id.ToString());
            var result = await Collection.DeleteOneAsync(filter);
            
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при удалении материала: {ex.Message}", ex);
        }
    }

    private static Material MapToMaterial(BsonDocument document)
    {
        var name = new MaterialName(document["name"].AsString);
        var subjectId = new SubjectId(Guid.Parse(document["subjectId"].AsString));
        var userId = new UserId(Guid.Parse(document["userId"].AsString));
        var year = new StudyYear(document["year"].AsInt32);
        var size = new MaterialSize(document["size"].AsInt64);
        var materialType = Enum.Parse<MaterialType>(document["materialType"].AsString, ignoreCase: true);
        var filePath = new ResourceLocation(document["filePath"].AsString);
        
        var semester = new Semester(document.Contains("semester") ? document["semester"].AsInt32 : 1);
        var description = document.Contains("description") ? document["description"].AsString : string.Empty;

        var material = new Material(name, subjectId, userId, year, semester, description, size, materialType, filePath);
        
        var idProperty = typeof(Material).GetProperty("Id");
        var uploadedAtProperty = typeof(Material).GetProperty("UploadedAt");
        
        idProperty?.SetValue(material, new MaterialId(Guid.Parse(document["materialId"].AsString)));
        uploadedAtProperty?.SetValue(material, document["uploadedAt"].ToUniversalTime());

        return material;
    }
    
    private static BsonDocument MapToBsonDocument(Material material)
    {
        if (material == null)
            throw new ArgumentNullException(nameof(material), "Material не должен быть null");
    
        return new BsonDocument
        {
            { "materialId", material.Id.Value.ToString() },
            { "subjectId", material.SubjectId.Value.ToString() },
            { "userId", material.UserId.Value.ToString() },
            { "name", material.Name.Value },
            { "year", material.Year.Value },
            { "semester", material.Semester.Value },
            { "description", material.Description },
            { "size", material.Size.Size },
            { "materialType", material.MaterialType.ToString() },
            { "filePath", material.FilePath.Value },
            { "uploadedAt", material.UploadedAt }
        };
    }
}