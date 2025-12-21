using Xunit;
using FIIT_folder.Infrastructure.FileStorage;
using FIIT_folder.Domain.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FIIT_folder.Infrastructure.Tests
{
    [TestFixture]
    public class StudyMaterialMongoDB_Should
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private StudyMaterialMongoDB _repository;
    
        private const string TestDbName = "StudyMaterialTestDb";
    
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
            var material = new StudyMaterial
            {
                Id = "1",
                Title = "Test Material",
                Description = "Description",
            };
    
            await _repository.AddAsync(material);
    
            var result = await _repository.GetByIdAsync("1");
    
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Material", result.Title);
        }
    
        [Test]
        public async Task GetStudyMaterial_Should_ReturnNullIfNotExists()
        {
            var result = await _repository.GetByIdAsync("nonexistent");
            Assert.IsNull(result);
        }
    }
}