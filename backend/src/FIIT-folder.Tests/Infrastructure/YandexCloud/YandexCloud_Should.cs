using NUnit.Framework;
using Moq;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Infrastructure.FileStorage;

[TestFixture]
public class YandexCloud_Should //не равботает пока можно закоментить
{
    private Mock<IAmazonS3> S3Mock = null;
    private FileStorageRepository Repository = null;
    private const string Bucket = "test-bucket";

    [SetUp]
    public void SetUp()
    {
        S3Mock = new Mock<IAmazonS3>();
        Repository = new FileStorageRepository(S3Mock.Object, Bucket);
     }

     [Test]
     public async Task SaveFile_Should()
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        var fileName = "file-test.txt";
        var folder = "subject-test";
        var path = await Repository.SaveFile(
            fileName,
            stream.Length,
            "text/plain",
            stream,
            folder
        );
        Assert.That(path, Is.EqualTo("subject-test/file-test.txt"));
    }
}