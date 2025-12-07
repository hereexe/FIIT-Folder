using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.FileStorage;

public class MaterialMongoDB : IMaterialMongoDB
{
    private readonly IMongoCollection<BsonDocument> CollectionStudyMaterial;
    
    public MaterialMongoDB(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        CollectionStudyMaterial = database.GetCollection<BsonDocument>("StudyMaterials");
        Console.WriteLine($"MongoDB подключен: {databaseName}");
        
        //CreateIndexes();
    }
    
    //public async Task<StudyMaterial> CreateMaterial(StudyMaterial material)
    // private void CreateIndexes()
    // {
    //     try
    //     {
    //         var idIndex = Builders<StudyMaterial>.IndexKeys.Ascending(m => m.Id);
    //         StudyMaterials.Indexes.CreateOne(new CreateIndexModel<StudyMaterial>
    //             (idIndex, new CreateIndexOptions { Unique = true })
    //         );
    //         Console.WriteLine("Создан уникальный индекс по Id");
    //         
    //         var subjectIdIndex = Builders<StudyMaterial>.IndexKeys.Ascending(m => m.SubjectId);
    //         StudyMaterials.Indexes.CreateOne(
    //             new CreateIndexModel<StudyMaterial>(subjectIdIndex)
    //         );
    //         Console.WriteLine("Создан индекс по SubjectId");
    //         
    //         var userIdIndex = Builders<StudyMaterial>.IndexKeys.Ascending(m => m.UserId);
    //         StudyMaterials.Indexes.CreateOne(
    //             new CreateIndexModel<StudyMaterial>(userIdIndex)
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

    public async Task<StudyMaterial> CreateStudyMaterial(StudyMaterial material)
    {
        try
        {
            if (material == null)
                throw new ArgumentNullException(nameof(material), "StudyMaterial не должен быть null");
            
            // var existing = await StudyMaterials
            //     .Find(m => m.Id == material.Id)
            //     .FirstOrDefaultAsync();
            
            // if (existing != null)
            //     throw new InvalidOperationException("StudyMaterial с таким id уже есть!");
            
            var bsonDocument = new BsonDocument
            {
                { "materialId", material.Id.Value.ToString() },
                { "subjectId", material.SubjectId.Value.ToString() },
                { "userId", material.UserId.Value.ToString() },
                { "name", material.Name.Value },
                { "year", material.Year.Value },
                { "size", material.Size.Size },
                { "materialType", material.MaterialType.ToString() },
                { "filePath", material.FilePath.Value },
                { "uploadedAt", material.UploadedAt }
            };
            
            await CollectionStudyMaterial.InsertOneAsync(bsonDocument);
            Console.WriteLine($"Материал сохранен в MongoDB!");
            return material;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.GetType().Name);
            throw;
        }
    }

    public async Task<StudyMaterial?> GetByIdStudyMaterial(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("materialId", id.ToString());
            var document = await CollectionStudyMaterial.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                Console.WriteLine("гавно");
                return null;
            }

            return MapToStudyMaterial(document);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении материала: {ex.Message}", ex);
        }
    }
    
    public async Task<StudyMaterial?> GetByNameStudyMaterial(string studyMaterialName)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("name", studyMaterialName);
            var document = await CollectionStudyMaterial.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                Console.WriteLine("гавно");
                return null;
            }

            return MapToStudyMaterial(document);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении материала: {ex.Message}", ex);
        }
    }

    public async Task<List<StudyMaterial>> GetBySubjectId(Guid subjectId)
    {
        return null;
    }

    public Task<StudyMaterial> UpdateStudyMaterial(StudyMaterial material)
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

    public async Task<List<StudyMaterial>> GetAll()
    {
        return null;
    }

    public Task<StudyMaterial> Create(StudyMaterial material)
    {
        throw new NotImplementedException();
    }

    // public Task<bool> DeleteStudyMaterial(string id)
    // {
    //     try
    //     {
    //         //var documents = await _collection.Find(new BsonDocument()).ToListAsync();
    //         //return documents.Select(MapToStudyMaterial).ToList();
    //         return null;
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception($"Ошибка при получении списка материалов: {ex.Message}", ex);
    //     }
    // }

    public async Task<bool> DeleteStudyMaterial(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("materialId", id.ToString());
            var result = await CollectionStudyMaterial.DeleteOneAsync(filter);
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