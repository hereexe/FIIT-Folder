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
            // ===== 1 СЕМЕСТР =====
            CreateSubject("Математический анализ", 1, 
                MaterialType.Exam, MaterialType.Colloquium, MaterialType.Pass),
            
            CreateSubject("Алгебра и геометрия", 1, 
                MaterialType.Exam, MaterialType.ControlWork, MaterialType.ComputerPractice),
            
            CreateSubject("Введение в математику", 1, 
                MaterialType.ControlWork),
            
            CreateSubject("Языки и технологии программирования", 1, 
                MaterialType.Exam),
            
            CreateSubject("Основы российской государственности", 1, 
                MaterialType.Pass),
            
            CreateSubject("Основы проектной деятельности", 1, 
                MaterialType.Pass),

            CreateSubject("Nand to Tetris", 1, 
                MaterialType.Exam),

            CreateSubject("ПЭК", 1, 
                MaterialType.Pass),

            // ===== 2 СЕМЕСТР =====
            CreateSubject("Математический анализ", 2, 
                MaterialType.Exam, MaterialType.Colloquium, MaterialType.Pass),
            
            CreateSubject("Алгебра и геометрия", 2, 
                MaterialType.Exam, MaterialType.ControlWork, MaterialType.ComputerPractice),
            
            CreateSubject("Языки и технологии программирования", 2, 
                MaterialType.Exam),
            
            CreateSubject("Философия", 2, 
                MaterialType.Pass),

            CreateSubject("Nand to Tetris", 2, 
                MaterialType.Exam),

            CreateSubject("Python", 2, 
                MaterialType.Exam),

            // ===== 3 СЕМЕСТР =====
            CreateSubject("Математический анализ", 3, 
                MaterialType.Exam, MaterialType.Colloquium, MaterialType.Pass),

            CreateSubject("Дискретная математика", 3, 
                MaterialType.Exam, MaterialType.ControlWork, MaterialType.ComputerPractice),

            CreateSubject("Сети и протоколы интернета", 3, 
                MaterialType.Pass),

            CreateSubject("Теория вероятности", 3, 
                MaterialType.Exam, MaterialType.ControlWork),

            CreateSubject("Python", 3, 
                MaterialType.Exam),
            
            // ===== 4 СЕМЕСТР =====
            CreateSubject("Теория вероятности", 4, 
                MaterialType.Exam, MaterialType.ControlWork)
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
