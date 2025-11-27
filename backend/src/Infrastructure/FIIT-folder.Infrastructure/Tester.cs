using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using FIIT_folder.Infrastructure.FileStorage;

namespace FIIT_folder.Infrastructure.Test
{
    public static class StorageTester
    {
        public static string PathInCloud(string name, string folder)
        {
            return string.IsNullOrEmpty(folder) ? name : $"{folder.Trim('/')}/{name}";
        }
        
        public static FileStorageRepository GetRepository()
        {
            return new FileStorageRepository(
                "YCAJEJjZUAxs4F0iCpajJG4_L",
                "YCNbbq1t3RGwiRuNrNAnCsODmPVgWFM1s6jT201L",  
                "my-fiit",
                "ru-central1"
            );
        }
        
        public static async Task TestDeleteFile()
        {
            var repository = GetRepository();
            var name = "aboba.txt";
            var folder = "";
            var filePath = PathInCloud(name, folder);
            await repository.DeleteFile(filePath);
            
            Console.WriteLine("Файл удален");
        }
        
        public static async Task TestSaveInRepository()
        {
            var repository = GetRepository();
            try
            {
                Console.WriteLine("Тест на сохранение");

                var testContent = "Простой тестовый файл";
                var bytes = Encoding.UTF8.GetBytes(testContent);
                var stream = new MemoryStream(bytes);
                
                await repository.SaveFile(
                    "aboba.txt",
                    bytes.Length,
                    "text/plain",
                    stream,
                    ""
                );
                
                Console.WriteLine($"Файл сохранен");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }
        }
        
        public static async Task TestFileIsCloud()
        {
            try
            {
                Console.WriteLine("Тест на существование уже созданного файла");
                var folder = "";
                var name = "aboba.txt";
                
                var pathInCloud = PathInCloud(name, folder);

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
            catch (Exception ex)
            {
                
            }
        }
    }
}