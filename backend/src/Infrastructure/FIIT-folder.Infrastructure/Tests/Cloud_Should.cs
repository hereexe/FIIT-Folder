using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using FIIT_folder.Infrastructure.FileStorage;
using Moq;
using NUnit.Framework;

namespace FIIT_folder.Infrastructure.Tests;

[TestFixture]
public class FileStorageRepository_Should
{
    private Mock<IAmazonS3> _s3Mock;
    private FileStorageRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _s3Mock = new Mock<IAmazonS3>();
        _repository = new FileStorageRepository(
            _s3Mock.Object,
            "dddddddd"
        );
    }
    
    [Test]
    public void CreatePathInCloud_should()
    {
        var path = FileStorageRepository.CreatePathInCloud("Ghfrnbrf1.txt", "docs");
        Assert.That(path, Is.EqualTo("Матан/file.txt"));
    }

    [Test]
    public async Task Return_true_if_file_exists()
    {
        _s3Mock
            .Setup(x => x.GetObjectMetadataAsync(
                "Матан",
                "Практика1.txt",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetObjectMetadataResponse());
        
        var result = await _repository.IsFileInRepository("Практика1.txt");
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task file_not_in()
    {
        _s3Mock
            .Setup(x => x.GetObjectMetadataAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new AmazonS3Exception("Not found"));
        
        var result = await _repository.IsFileInRepository("Практика1.txt");
        Assert.That(result, Is.False);
    }
}