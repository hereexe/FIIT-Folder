using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.FileStorage;

public class MaterialMongoDB : IMaterialMongoDB
{
    private readonly IMongoCollection<BsonDocument> _collection;
    private readonly IMongoCollection<StudyMaterial> StudyMaterials;
    
    public MaterialMongoDB(IConfiguration configuration)
    {
        var connectionString = configuration["MongoDbSettings:ConnectionString"];
        var name = configuration["MongoDbSettings:DatabaseName"];
            
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(name);
        StudyMaterials = database.GetCollection<StudyMaterial>(name);
        Console.WriteLine("MongoDB подключен!");
        
        //CreateIndexes();
    }
    
    public MaterialMongoDB(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        StudyMaterials = database.GetCollection<StudyMaterial>("StudyMaterials");
        Console.WriteLine($"MongoDB подключен: {databaseName}");
        
        //CreateIndexes();
    }
    
    public async Task<StudyMaterial> CreateMaterial(StudyMaterial material)
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
            await StudyMaterials.InsertOneAsync(material);
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

    public Task<StudyMaterial> UpdateStudyMaterial(StudyMaterial material)
    {
        throw new NotImplementedException();
    }

    public Task<StudyMaterial> GetByIdStudyMaterial(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteStudyMaterial(string id)
    {
        throw new NotImplementedException();
    }
}