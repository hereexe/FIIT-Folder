using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FIIT_folder.Infrastructure.FileStorage;

public class SubjectMongoDB : ISubjectRepository
{
    private readonly IMongoCollection<BsonDocument> _collection;

    public SubjectMongoDB(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<BsonDocument>("subjects");
    }

    public async Task<Subject> Create(Subject subject)
    {
        try
        {
            var document = new BsonDocument
            {
                { "subjectId", subject.Id.Value.ToString() },
                { "name", subject.Name.Value },
                { "semester", subject.Semester.Value },
                { "materialTypes", new BsonArray(subject.AvailableMaterialTypes.Select(t => t.ToString())) }
            };

            await _collection.InsertOneAsync(document);
            Console.WriteLine($"Предмет '{subject.Name.Value}' сохранён в MongoDB");
            return subject;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при сохранении предмета: {ex.Message}", ex);
        }
    }

    public async Task<Subject?> GetById(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("subjectId", id.ToString());
            var document = await _collection.Find(filter).FirstOrDefaultAsync();

            if (document == null)
                return null;

            return MapToSubject(document);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении предмета: {ex.Message}", ex);
        }
    }

    public async Task<List<Subject>> GetAll()
    {
        try
        {
            var documents = await _collection.Find(new BsonDocument()).ToListAsync();
            return documents.Select(MapToSubject).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении списка предметов: {ex.Message}", ex);
        }
    }

    public async Task<bool> Update(Subject subject)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("subjectId", subject.Id.Value.ToString());
            var update = Builders<BsonDocument>.Update
                .Set("name", subject.Name.Value)
                .Set("semester", subject.Semester.Value)
                .Set("materialTypes", new BsonArray(subject.AvailableMaterialTypes.Select(t => t.ToString())));

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при обновлении предмета: {ex.Message}", ex);
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("subjectId", id.ToString());
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при удалении предмета: {ex.Message}", ex);
        }
    }

    public async Task<List<Subject>> GetByName(string name)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("name", name);
            var documents = await _collection.Find(filter).ToListAsync();
            return documents.Select(MapToSubject).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при получении предметов по имени: {ex.Message}", ex);
        }
    }

    public async Task<bool> ExistsByName(string name)
    {
        try
        {
            var filter = Builders<BsonDocument>.Filter.Eq("name", name);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при проверке существования предмета: {ex.Message}", ex);
        }
    }

    private static Subject MapToSubject(BsonDocument document)
    {
        var id = new SubjectId(Guid.Parse(document["subjectId"].AsString));
        var name = new SubjectName(document["name"].AsString);
        var semester = new SubjectSemester(document["semester"].AsInt32);
        
        var materialTypes = document["materialTypes"].AsBsonArray
            .Select(t => Enum.Parse<MaterialType>(t.AsString, ignoreCase: true))
            .ToList();

        return new Subject(id, name, semester, materialTypes);
    }
}