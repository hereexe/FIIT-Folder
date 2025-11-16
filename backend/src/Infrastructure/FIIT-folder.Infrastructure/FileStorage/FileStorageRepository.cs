using FIIT_folder.Domain.Interfaces;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace FIIT_folder.Infrastructure.FileStorage;

public class FileStorageRepository : IFileStorageRepository
{
    private readonly IAmazonS3 S3Client;
    private readonly string BucketName;
    private readonly string ServiceUrl;
    
    public FileStorageRepository(string accessKey, string secretKey, string bucketName, string region = "ru-central1")
    {
        BucketName = bucketName;
        ServiceUrl = "https://storage.yandexcloud.net";
        
        var config = new AmazonS3Config
        {
            ServiceURL = ServiceUrl,
            AuthenticationRegion = region,
            ForcePathStyle = true
        };

        S3Client = new AmazonS3Client(accessKey, secretKey, config);
        
        InitializeBucketAsync().Wait();
    }
    
    private async Task InitializeBucketAsync()
    {
        try
        {
            var bucketExists = await S3Client.DoesS3BucketExistAsync(BucketName);
            if (!bucketExists)
                throw new InvalidOperationException($"Бакет '{BucketName}' " +
                                                    $"не существует в Yandex Object Storage. Создайте его через консоль управления.");
            
            Console.WriteLine("Облако - контейнер инициализировано: {BucketName}");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка инициализации: {ex.Message}", ex);
        }
    }
    
    public async Task<string> SaveFile(string name, long size, string type, Stream content, string folder)
    {
        try
        {
            var safeFileName = name;
            string objectKey;
            
            if (string.IsNullOrEmpty(folder))
                objectKey = safeFileName;
            else
                objectKey = $"{folder.Trim('/')}/{safeFileName}";
            
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = content,
                BucketName = BucketName,
                Key = objectKey,
                ContentType = type,
                AutoCloseStream = false
            };
            var transferUtility = new TransferUtility(S3Client);
            await transferUtility.UploadAsync(uploadRequest);

            Console.WriteLine($"Файл загружен в облако чиназес: {objectKey}");
            
            return objectKey;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка загрузки файла '{name}' в облако плаки плаки: {ex.Message}", ex);
        }
    }

    public Task<Stream> GetFile(string fullPathFile)
    {
        return null;
    }

    public Task DeleteFile(string fullPathFile)
    {
        return null;
    }

    public Task<bool> IsFileInRepository(string fullPathFile)
    {
        return null;
    }
}