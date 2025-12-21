using NUnit.Framework;
using Moq;
using MongoDB.Driver;
using MongoDB.Bson;
using FIIT_folder.Infrastructure.FileStorage;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

[TestFixture]
public class StudyMaterialMongoDB
{
    private Mock<IMongoCollection<BsonDocument>> _collectionMock = null;
    private MaterialMongoDB _repository = null;

    [SetUp]
    public void SetUp()
    {
        _collectionMock = new Mock<IMongoCollection<BsonDocument>>();
        _repository = new MaterialMongoDB(_collectionMock.Object);
    }

    [Test]
    public async Task CreateStudyMaterial_Should()
    {
        var material = new StudyMaterial(
            new MaterialName("Матан"),
            new SubjectId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()),
            new StudyYear(2024),
            new Semester(1),
            "dsdwf",
            new MaterialSize(12233),
            MaterialType.Exam,
            new ResourceLocation("path/test.pdf")
        );
        _collectionMock
            .Setup(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, default))
            .Returns(Task.CompletedTask);
        var result = await _repository.CreateStudyMaterial(material);
        
        Assert.That(result, Is.Not.Null);

        _collectionMock.Verify(c => c.InsertOneAsync(It.IsAny<BsonDocument>(),
            null, default), Times.Once);
    }
}