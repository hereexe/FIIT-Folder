using NUnit.Framework;
using Moq;
using MongoDB.Driver;
using MongoDB.Bson;
using FIIT_folder.Infrastructure.FileStorage;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

[TestFixture]
public class MaterialMongoDB_Should
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
    public async Task CreateMaterial_Should()
    {
        var material = new Material(
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
        var result = await Repository.CreateMaterial(material);
        
        Assert.That(result, Is.Not.Null);

        CollectionMock.Verify(c => c.InsertOneAsync(It.IsAny<BsonDocument>(),
            null, default), Times.Once);
    }
    
    [Test]
    public async Task DeleteMaterial_Should()
    {
        var materialId = Guid.NewGuid();
        var deleteResult = new Mock<DeleteResult>();
        deleteResult.Setup(d => d.DeletedCount).Returns(0);
        
        CollectionMock.Setup(c => c.DeleteOneAsync(It.Is<FilterDefinition<BsonDocument>>
                (f => true), default)).ReturnsAsync(deleteResult.Object);
        var result = await Repository.DeleteMaterial(materialId);
        Assert.That(result, Is.False);
        CollectionMock.Verify(c => c.DeleteOneAsync(
            It.Is<FilterDefinition<BsonDocument>> (f => true), default), Times.Once);
    }
    
    // [Test]
    // public async Task GetBySubjectId_Should_Return_Materials_List()
    // {
    //     // Arrange
    //     var subjectId = Guid.NewGuid();
    //     var materialId1 = Guid.NewGuid();
    //     var materialId2 = Guid.NewGuid();
    //     
    //     var bsonDocument1 = new BsonDocument
    //     {
    //         { "materialId", materialId1.ToString() },
    //         { "subjectId", subjectId.ToString() },
    //         { "userId", Guid.NewGuid().ToString() },
    //         { "name", "Матан 1" },
    //         { "year", 2024 },
    //         { "semester", 1 },
    //         { "description", "Тест 1" },
    //         { "size", 1000 },
    //         { "materialType", "Exam" },
    //         { "filePath", "path/test1.pdf" },
    //         { "uploadedAt", DateTime.UtcNow }
    //     };
    //
    //     var bsonDocument2 = new BsonDocument
    //     {
    //         { "materialId", materialId2.ToString() },
    //         { "subjectId", subjectId.ToString() },
    //         { "userId", Guid.NewGuid().ToString() },
    //         { "name", "Матан 2" },
    //         { "year", 2024 },
    //         { "semester", 2 },
    //         { "description", "Тест 2" },
    //         { "size", 2000 },
    //         { "materialType", "Colloquium" },
    //         { "filePath", "path/test2.pdf" },
    //         { "uploadedAt", DateTime.UtcNow }
    //     };
    //
    //     CollectionMock
    //         .Setup(c => c.FindAsync(
    //             It.Is<FilterDefinition<BsonDocument>>(f => true),
    //             null,
    //             default))
    //         .ReturnsAsync(() => 
    //         {
    //             var mockCursor = new Mock<IAsyncCursor<BsonDocument>>();
    //             mockCursor.Setup(_ => _.Current).Returns(new[] { bsonDocument1, bsonDocument2 });
    //             mockCursor.SetupSequence(_ => _.MoveNext(default))
    //                      .Returns(true)
    //                      .Returns(false);
    //             return mockCursor.Object;
    //         });
    //
    //     // Act
    //     var result = await Repository.GetBySubjectId(subjectId);
    //     
    //     // Assert
    //     Assert.That(result, Is.Not.Null);
    //     Assert.That(result.Count, Is.EqualTo(2));
    //     Assert.That(result[0].SubjectId.Value, Is.EqualTo(subjectId));
    //     Assert.That(result[1].SubjectId.Value, Is.EqualTo(subjectId));
    //     
    //     CollectionMock.Verify(c => c.FindAsync(
    //         It.Is<FilterDefinition<BsonDocument>>(f => true),
    //         null,
    //         default), Times.Once);
    // }
}