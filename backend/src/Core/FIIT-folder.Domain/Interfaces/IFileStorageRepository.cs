namespace FIIT_folder.Domain.Interfaces;

public interface IFileStorageRepository
{
    Task <string> SaveFile(string name, long size, string type, Stream content, string path); //возвращает полный путь к файлу
    Task<Stream> GetFile(string fullPathFile); //Task вып асинхроно
    
    Task DeleteFile(string fullPathFile);
    
    Task<bool> IsFileInRepository(string fullPathFile);
}