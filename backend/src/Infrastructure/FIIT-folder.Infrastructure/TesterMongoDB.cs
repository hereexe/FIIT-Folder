using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Infrastructure.FileStorage;
using Microsoft.Extensions.Configuration;
using FIIT_folder.Domain.Entities;
using System.IO;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Infrastructure.Test;

public class TesterMongoDB
{
    //private readonly IMaterialMongoDB Repository;
    
    public static MaterialMongoDB GetRepository(string connectionString = null, string databaseName = null)
    {
        connectionString ??= "mongodb://localhost:27017";
        databaseName ??= "newbd";
        
        return new MaterialMongoDB(
            connectionString,
            databaseName
        );
    }
    
    public static async Task CreateMaterial()
    {
        try
        {
            Console.WriteLine("Тест на сохранение в MongoBD");
            
            var repository = GetRepository();
            
            Console.WriteLine("Создаю новый Material");
            
            var material = new Material(
                new MaterialName("Матан"), 
                new SubjectId(Guid.NewGuid()), 
                new UserId(Guid.NewGuid()), 
                new StudyYear(2024),
                new Semester(1), // Semester
                "Тестовое описание", // Description
                new MaterialSize(2000), 
                MaterialType.Colloquium, 
                new ResourceLocation("/путь/к/файлу.pdf")
            );
            
            Console.WriteLine("Пытаюсь сохранить новый Material");
            var createdMaterial = await repository.CreateMaterial(material);
            
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
    
    public static async Task DeleteMaterial()
    {
        try
        {
            Console.WriteLine("Тест на удаление в MongoBD");
            
            var repository = GetRepository();
            
            
            Console.WriteLine("Пытаюсь удалить Material, который ранее создавал");
            var flag = await repository.DeleteMaterial(new Guid(
                "5cf635fe-7f4e-45a1-89e6-71e30c702dab"));
            
            if (flag)
                Console.WriteLine($"Материал успешно удален из MongoDB!");
            else
                Console.WriteLine("Error Ошибка удаления");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine("Какая то фигня");
        }
    }
    
    public static async Task GetByIdMaterial(Guid guid)
    {
        try
        {
            Console.WriteLine("Тест на поиск в MongoBD");
            var repository = GetRepository();
            Console.WriteLine("Пытаюсь найти Material(который ранее создавал) по id");
            var flag = await repository.GetByIdMaterial(guid);

            if (flag != null)
                Console.WriteLine("материал нашелся! + в domain получаю его");
            else
                Console.WriteLine("материала в базе нет!");
        }
        
        catch (ArgumentNullException ex)
        {
            Console.WriteLine("Ошибка при поиске");
        }
    }
    
    public static async Task GetByNameMaterial(string MaterialName)
    {
        try
        {
            Console.WriteLine("Тест на поиск в MongoBD по имени");
            var repository = GetRepository();
            Console.WriteLine("Пытаюсь найти Material(который ранее создавал) по MaterialName");
            var flag = await repository.GetByNameMaterial(MaterialName);

            if (flag != null)
                Console.WriteLine("материал нашелся! + в domain получаю его");
            else
                Console.WriteLine("материала в базе нет!");
        }
        
        catch (ArgumentNullException ex)
        {
            Console.WriteLine("Ошибка при поиске");
        }
    }
    
    public static async Task IsMaterialInMongoDB()
    {
        
    }
}