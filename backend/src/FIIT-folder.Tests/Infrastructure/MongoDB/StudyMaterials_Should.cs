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
    private Mock<IMongoCollection<BsonDocument>> CollectionMock = null;
    private MaterialMongoDB Repository = null;

    [SetUp]
    public void SetUp()
    {
        CollectionMock = new Mock<IMongoCollection<BsonDocument>>();
        Repository = new MaterialMongoDB(CollectionMock.Object);
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
        CollectionMock
            .Setup(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, default))
            .Returns(Task.CompletedTask);
        var result = await Repository.CreateStudyMaterial(material);
        
        Assert.That(result, Is.Not.Null);

        CollectionMock.Verify(c => c.InsertOneAsync(It.IsAny<BsonDocument>(),
            null, default), Times.Once);
    }
    
    [Test]
    public async Task DeleteStudyMaterial_Should()
    {
        var materialId = Guid.NewGuid();
        var deleteResult = new Mock<DeleteResult>();
        deleteResult.Setup(d => d.DeletedCount).Returns(0);
        
        CollectionMock.Setup(c => c.DeleteOneAsync(It.Is<FilterDefinition<BsonDocument>>
                (f => true), default)).ReturnsAsync(deleteResult.Object);
        var result = await Repository.DeleteStudyMaterial(materialId);
        Assert.That(result, Is.False);
        CollectionMock.Verify(c => c.DeleteOneAsync(
            It.Is<FilterDefinition<BsonDocument>> (f => true), default), Times.Once);
    }
}