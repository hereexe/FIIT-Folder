using NUnit.Framework;
using Moq;
using Amazon.S3;
using Amazon.S3.Model;
using System.Text;

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
    public async Task SaveFile_Should_Return_Path()
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
        var fileName = "file.txt";
        var folder = "StudyMaterials";
        var path = await Repository.SaveFile(
            fileName,
            stream.Length,
            "text/plain",
            stream,
            folder
        );
        Assert.That(path, Is.EqualTo("Матан/file.txt"));
    }
}