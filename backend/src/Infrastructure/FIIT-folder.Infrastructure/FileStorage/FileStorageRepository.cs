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
            Console.WriteLine($"Подключение к Yandex Cloud Storage... Bucket: {BucketName}, ServiceUrl: {ServiceUrl}");
            
            var request = new ListBucketsRequest(); //сделал запрос на получение
            var response = await Client.ListBucketsAsync(request); //получаю список бакетов
            
            Console.WriteLine($"Найдено бакетов: {response.Buckets.Count}");
            foreach (var bucket in response.Buckets)
            {
                Console.WriteLine($"  - {bucket.BucketName}");
            }
            
            var bucketExists = response.Buckets.Any(bucket => bucket.BucketName == BucketName);//проверка на сущ
            
            if (!bucketExists) //бакета нет
            {
                Console.WriteLine($"ОШИБКА: Бакет '{BucketName}' не найден в списке доступных бакетов!");
                throw new InvalidOperationException($"Бакет '{BucketName}' не существует в облаке. Доступные бакеты: {string.Join(", ", response.Buckets.Select(b => b.BucketName))}");
            }
            Console.WriteLine($"Облако - контейнер '{BucketName}' инициализировано успешно");
        }
        catch (AmazonS3Exception s3Ex)
        {
            Console.WriteLine($"Ошибка S3: {s3Ex.ErrorCode} - {s3Ex.Message}");
            throw new InvalidOperationException($"Ошибка инициализации S3: {s3Ex.ErrorCode} - {s3Ex.Message}", s3Ex);
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            Console.WriteLine($"Ошибка инициализации хранилища: {ex.GetType().Name} - {ex.Message}");
            throw new InvalidOperationException($"Ошибка инициализации хранилища: {ex.Message}", ex);
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
                pathInCloud = CreatePathInCloud(safeFileName, folder);
            
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

            Console.WriteLine($"Файл загружен в облако с путем: {pathInCloud}");
            
            return pathInCloud; //возвращаем путь к файлику для MobgoBD
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Ошибка загрузки файла '{name}' в облако плаки плаки");
        }
    }

    public async Task<Stream> GetFile(string fullPathFile)
    {
        try
        {
            if (string.IsNullOrEmpty(fullPathFile))
                throw new ArgumentException("Путь к файлу не может быть пустым");

            var request = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = fullPathFile
            };

            var response = await Client.GetObjectAsync(request);
    
            // Копируем содержимое в MemoryStream для безопасного использования
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Возвращаем указатель в начало
    
            // Закрываем исходный поток
            response.ResponseStream.Close();
    
            Console.WriteLine($"Файл {fullPathFile} успешно скачан");
            return memoryStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"Файл {fullPathFile} не найден в облачном хранилище", ex);
        }
        catch (AmazonS3Exception ex)
        {
            throw new InvalidOperationException($"Ошибка при скачивании файла {fullPathFile}: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Неожиданная ошибка при скачивании файла {fullPathFile}: {ex.Message}", ex);
        }
    }

    public async Task DeleteFile(string fullPathFile)
    {
        try
        {
            if (string.IsNullOrEmpty(fullPathFile))
                throw new ArgumentException("Путь нулевой", nameof(fullPathFile));

            if (!await IsFileInRepository(fullPathFile))
                throw new ArgumentException("Файла в репеозитории нет!");
            
            var request = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = fullPathFile
            };

            await Client.DeleteObjectAsync(request);
            Console.WriteLine("Файл удален из облака");
        }
        catch (AmazonS3Exception ex)
        {
            throw new InvalidOperationException($"Ошибка удаления файла");
        }
    }
    
    public static string CreatePathInCloud(string name, string folder)
    {
        return string.IsNullOrEmpty(folder) ? name : $"{folder.Trim('/')}/{name}";
    }

    public async Task<bool> IsFileInRepository(string fullPathFile)
    {
        try
        {
            if (string.IsNullOrEmpty(fullPathFile))
                return false;

            await Client.GetObjectMetadataAsync(BucketName, fullPathFile);
            return true;
        }
        catch
        {
            Console.WriteLine("Файл не найден!");
            return false;
        }
    }
}