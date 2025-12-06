using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Infrastructure.FileStorage;
using Microsoft.Extensions.Configuration;
using FIIT_folder.Domain.Entities;
using System.IO;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Infrastructure.Test;

public class TesterMongoDB
{
    //private readonly IMaterialMongoDB Repository;
    
    public static MaterialMongoDB GetRepository(string connectionString = null, string databaseName = null)
    {
        return new MaterialMongoDB(
            "mongodb+srv://filt_app_user:FIIT2024Secure!" +
            "@cluster0.dhgee6e.mongodb.net/?appName=Cluster0",
            "Test-DB"
        );
    }
    
    public static async Task CreateMaterial()
    {
        try
        {
            Console.WriteLine("Тест на сохранение в MongoBD");
            
            var repository = GetRepository();

            var material = new StudyMaterial(new MaterialName("Матан"), new SubjectId(new Guid()), new UserId(new Guid()), 
                new StudyYear(2024), new MaterialSize(2000), MaterialType.Colloquium, new ResourceLocation(""));
                
            
            Console.WriteLine("Создаю новый StudyMaterial");
            
            var createdMaterial = await repository.CreateStudyMaterial(material);
            Console.WriteLine("Пытаюсь сохранить новый StudyMaterial");
            
            if (createdMaterial != null)
                Console.WriteLine($"Материал успешно создан в MongoDB!");
            else
                Console.WriteLine("Error Материал не был создан");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine("Ошибка создания");
        }
    }
    
    public async Task DeleteMaterial()
    {
        
    }
    
    public async Task GetMaterial()
    {
        
    }
    
    public async Task IsMaterialInMongoDB()
    {
        
    }
}