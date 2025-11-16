using FIIT_folder.Domain.Interfaces;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace FIIT_folder.Infrastructure.FileStorage;

public class FileStorageRepository : IFileStorageRepository
{
    private readonly IAmazonS3 Client; //клиент для облака
    private readonly string BucketName; //имя создаваемого бакета(нового контейнера)
    private readonly string ServiceUrl; //ссылка соответственно
    
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

        Client = new AmazonS3Client(accessKey, secretKey, config);
        
        InitializeBucketAsync().Wait();
    }
    
    private async Task InitializeBucketAsync()
    {
        try
        {
            var request = new ListBucketsRequest(); //сделал запрос на получение
            var response = await Client.ListBucketsAsync(request); //получаю список бакетов
            var bucketExists = response.Buckets.Any(bucket => bucket.BucketName == BucketName);//проверка на сущ
            
            if (!bucketExists) //бакета нет
                throw new InvalidOperationException($"Бакет не существует в Yandex Object Storage." +
                                                    $" Создайте его через консоль управления.");
            Console.WriteLine("Облако - контейнер инициализировано");
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
            string pathInCloud;
            
            if (string.IsNullOrEmpty(folder))
                pathInCloud = safeFileName;
            else
                pathInCloud = $"{folder.Trim('/')}/{safeFileName}";
            
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = content,
                BucketName = BucketName,
                Key = pathInCloud,
                ContentType = type,
                AutoCloseStream = false
            };
            var transferUtility = new TransferUtility(Client);
            await transferUtility.UploadAsync(uploadRequest);

            Console.WriteLine($"Файл загружен в облако чиназес: {pathInCloud}");
            
            return pathInCloud; //возвращаем путь к файлику для MobgoBD
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