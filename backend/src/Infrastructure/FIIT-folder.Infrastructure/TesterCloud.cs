using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using FIIT_folder.Infrastructure.FileStorage;

namespace FIIT_folder.Infrastructure.Test
{
    public static class StorageTester
    {
        public static FileStorageRepository GetRepository()
        {
            return new FileStorageRepository(
                "YCAJEJjZUAxs4F0iCpajJG4_L",
                "YCNbbq1t3RGwiRuNrNAnCsODmPVgWFM1s6jT201L",  
                "my-fiit",
                "ru-central1"
            );
        }
        
        public static async Task TestDeleteFile(FileStorageRepository repository, string name, string folder)
        {
            var filePath = FileStorageRepository.CreatePathInCloud(name, folder);
            await repository.DeleteFile(filePath);
            
            Console.WriteLine("Файл удален");
        }

        public static async Task TestGetFile(FileStorageRepository repository, string name, string folder)
        {
            Console.WriteLine("Тест скачивание файл");
            var filePath = FileStorageRepository.CreatePathInCloud(name, folder);
            if (await repository.IsFileInRepository(filePath))
            {
                using var downloadedStream = await repository.GetFile(filePath);
            }
            else
            {
                Console.WriteLine("Файла нет в репозитории!");
            }
        }
        
        public static async Task TestSaveInRepository(
            FileStorageRepository repository, string testContent, 
            string type, string name, string folder)
        {
            try
            {
                Console.WriteLine("Тест на сохранение");
                
                var bytes = Encoding.UTF8.GetBytes(testContent);
                var stream = new MemoryStream(bytes);
                
                await repository.SaveFile(
                    name,
                    bytes.Length,
                    type,
                    stream,
                    folder
                );
                
                Console.WriteLine($"Файл сохранен");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }
        }
        
        public static async Task TestFileIsCloud(string name, string folder)
        {
            try
            {
                Console.WriteLine("Тест на существование уже созданного файла");
                
                var pathInCloud = FileStorageRepository.CreatePathInCloud(name, folder);

                var repository = GetRepository();

                if (await repository.IsFileInRepository(pathInCloud))
                {
                    Console.WriteLine("Файл в облаке есть!");
                }
                else
                {
                    Console.WriteLine("Файл в облаке нет!");
                }
            }
            catch (Exception)
            {
                throw new Exception("Неизвестная ошибка при поиске файла");
            }
        }
    }
}