using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using FIIT_folder.Infrastructure.FileStorage;

namespace FIIT_folder.Infrastructure.Test
{
    public static class StorageTester
    {
        private static async Task TestDeleteFile(FileStorageRepository repository)
        {
            var filePath = "test-folder/test-file.txt";
            await repository.DeleteFile(filePath);
            
            Console.WriteLine("Файл удален");
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
        
        public static async Task TestOnlySave()
        {
            var repository = GetRepository();
            try
            {
                Console.WriteLine("Тест на сохранение");

                var testContent = "Простой тестовый файл";
                var bytes = Encoding.UTF8.GetBytes(testContent);
                var stream = new MemoryStream(bytes);
                
                await repository.SaveFile(
                    "test101.txt",
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
        
        public static async Task TestOnlyDelete()
        {
            
        }
        
        public static async Task TestFileIsCloud()
        {
            try
            {
                Console.WriteLine("Тест на существование уже созданного файла");
                var folder = "";
                var name = "test101.txt";
                
                var pathInCloud = string.IsNullOrEmpty(folder) ? name : $"{folder.Trim('/')}/{name}";

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