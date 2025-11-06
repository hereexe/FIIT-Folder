namespace FIIT_folder.Domain.Models;

public class Material
{
    public Guid Id { get; private set; } //Guid - создает уникальный идентефикатор
    public string Name { get; private set; }
    public string Path { get; private set; }
    public long Size { get; private set; }
    public string Type { get; private set; }
    public DateTime Date { get; private set; }
    
    //Далее идут огр на файл
    public long SizeLimit = 100 * 1024 * 1024; //Здесь добавляем лимит на размер файла
    public string[] ValidTypes = //Здесь добавляем допустимые типы
    {
        "pdf",
        "word",
    };

    private Material()
    {
        
    }
    
    public static Material Create(string name, long size, string type)
    {
        return new Material
        {
            Id = Guid.NewGuid(),
            Name = name,
            Size = size,
            Type = type,
            Date = DateTime.UtcNow
        };
    }
    
    public void SavePath(string path)
    {
        Path = path;
    }

    // тут проверки на валидность
    public bool IsValidSize(long size)
    {
        return size <= SizeLimit;
    }

    public bool IsValidType(string type)
    {
        return ValidTypes.Contains(type);
    }
}