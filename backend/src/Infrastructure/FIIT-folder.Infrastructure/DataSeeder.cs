using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Infrastructure;

public class DataSeeder
{
    private readonly ISubjectRepository _subjectRepository;

    public DataSeeder(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task SeedAsync()
    {
        var existingSubjects = await _subjectRepository.GetAll();
        if (existingSubjects.Count > 0)
        {
            Console.WriteLine("База данных уже содержит предметы. Seed пропущен.");
            return;
        }

        var subjects = GetSubjectsToSeed();

        foreach (var subject in subjects)
        {
            await _subjectRepository.Create(subject);
            Console.WriteLine($"Создан предмет: {subject.Name.Value}");
        }

        Console.WriteLine($"Seed завершён. Создано предметов: {subjects.Count}");
    }

    private List<Subject> GetSubjectsToSeed()
    {
        return new List<Subject>
        {
            // 1 семестр
            CreateSubject("Математический анализ", 1, 
                MaterialType.Exam, MaterialType.Colloquium, MaterialType.Pass),
            
            CreateSubject("Алгебра и геометрия", 1, 
                MaterialType.Exam, MaterialType.Colloquium),
            
            CreateSubject("Дискретная математика", 1, 
                MaterialType.Exam, MaterialType.Colloquium),
            
            CreateSubject("Введение в математику", 1, 
                MaterialType.Pass),
            
            CreateSubject("Языки и технологии программирования", 1, 
                MaterialType.Exam, MaterialType.ControlWork, MaterialType.ComputerPractice),
            
            CreateSubject("Основы российской государственности", 1, 
                MaterialType.Pass),
            
            CreateSubject("Английский язык", 1, 
                MaterialType.Pass),
            
            CreateSubject("Основы проектной деятельности", 1, 
                MaterialType.Pass),

            // 2 семестр
            CreateSubject("Теория вероятности", 2, 
                MaterialType.Exam, MaterialType.Colloquium),
            
            CreateSubject("Философия", 2, 
                MaterialType.Exam),
            
            CreateSubject("Nand to Tetris", 2, 
                MaterialType.Pass, MaterialType.ComputerPractice),
            
            CreateSubject("Сети и протоколы интернета", 2, 
                MaterialType.Exam, MaterialType.ComputerPractice),
        };
    }

    private Subject CreateSubject(string name, int semester, params MaterialType[] materialTypes)
    {
        return new Subject(
            new SubjectId(Guid.NewGuid()),
            new SubjectName(name),
            new SubjectSemester(semester),
            materialTypes
        );
    }
}
