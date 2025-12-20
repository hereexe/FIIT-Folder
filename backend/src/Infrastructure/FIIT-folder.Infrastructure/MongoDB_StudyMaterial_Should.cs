using NUnit.Framework;
using FIIT_folder.Infrastructure.FileStorage;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using MongoDB.Driver;
using FIIT_folder.Domain.Enums;
using System.Threading.Tasks;

namespace FIIT_folder.Infrastructure.Tests;

[TestFixture]
public class StudyMaterialMongoDB_Should
{
    private IMongoClient _client;
    private IMongoDatabase _database;
    private StudyMaterialMongoDB _repository;

    private const string TestDbName = "FIIT-Folder-Test";

    [SetUp]
    public void SetUp()
    {
        _client = new MongoClient("mongodb://localhost:27017");
        _database = _client.GetDatabase(TestDbName);
        
        _database.DropCollection("StudyMaterials");

        
        _repository = new StudyMaterialMongoDB(_database);
    }

    [TearDown]
    public void TearDown()
    {
        _client.DropDatabase(TestDbName);
    }

    [Test]
    public async Task AddStudyMaterial_Should_InsertDocument()
    {
        var material = new StudyMaterial(
            new MaterialName("Матан"), 
            new SubjectId(Guid.NewGuid()), 
            new UserId(Guid.NewGuid()), 
            new StudyYear(2024),
            1, // Semester
            "Тестовое описание", // Description
            new MaterialSize(2000), 
            MaterialType.Colloquium, 
            new ResourceLocation("/путь/к/файлу.pdf")
        );

        await _repository.CreateStudyMaterial(material);

        var result = await _repository.GetByIdStudyMaterial("1");

        Assert.IsNotNull(result);
        Assert.AreEqual("Test Material", result.Title);
    }

    [Test]
    public async Task GetStudyMaterial_Should_ReturnNullIfNotExists()
    {
        var result = await _repository.GetByIdStudyMaterial("nonexistent");
        Assert.IsNull(result);
    }
}