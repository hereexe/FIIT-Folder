using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using FIIT_folder.Infrastructure.FileStorage;

namespace FIIT_folder.Infrastructure.Test
{
    public static class StorageTester
    {
        private static async Task TestSaveFile(FileStorageRepository repository)
        {
            var testContent = "" + DateTime.Now;
            var bytes = Encoding.UTF8.GetBytes(testContent);
            var stream = new MemoryStream(bytes);
            
            var filePath = await repository.SaveFile(
                "test-file.txt",
                bytes.Length,
                "text/plain", 
                stream,
                "test-folder"
            );
            
            Console.WriteLine($"Файл сохранен найс: {filePath}");
        }

        private static async Task TestFileExists(FileStorageRepository repository)
        {
            var filePath = "test-folder/test-file.txt";
            var exists = await repository.IsFileInRepository(filePath);
            
            Console.WriteLine($"Файл существует найс: {exists}");
        }

        private static async Task TestGetFile(FileStorageRepository repository)
        {
            var filePath = "test-folder/test-file.txt";
            var stream = await repository.GetFile(filePath);
            if (stream != null)
                Console.WriteLine($"Файл получен с размером: {stream.Length} байт");
            else
                Console.WriteLine("Такого файла нет(");
        }

        private static async Task TestDeleteFile(FileStorageRepository repository)
        {
            var filePath = "test-folder/test-file.txt";
            await repository.DeleteFile(filePath);
            
            Console.WriteLine("Файл удален");
        }
        
        public static async Task TestOnlySave()
        {
            try
            {
                Console.WriteLine("Тест на сохранение");
                
                var repository = new FileStorageRepository(
                    "YCAJEJjZUAxs4F0iCpajJG4_L",
                    "YCNbbq1t3RGwiRuNrNAnCsODmPVgWFM1s6jT201L",  
                    "my-fiit",
                    "ru-central1"
                );

                var testContent = "Простой тестовый файл " + DateTime.Now;
                var bytes = Encoding.UTF8.GetBytes(testContent);
                var stream = new MemoryStream(bytes);
                
                var filePath = await repository.SaveFile(
                    "test1.txt",
                    bytes.Length,
                    "text/plain",
                    stream,
                    ""
                );
                
                Console.WriteLine($"Файл сохранен: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }
        }
    }
}