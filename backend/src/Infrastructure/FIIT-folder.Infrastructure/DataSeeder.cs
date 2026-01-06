using FIIT_folder.Application.Interfaces;
using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Infrastructure;

public class DataSeeder
{
    private readonly ISubjectRepository _subjectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public DataSeeder(
        ISubjectRepository subjectRepository,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _subjectRepository = subjectRepository;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        await SeedSubjectsAsync();
        await SeedAdminAsync();
    }

    private async Task SeedSubjectsAsync()
    {
        var existingSubjects = await _subjectRepository.GetAll();
        if (existingSubjects.Count > 0)
        {
            Console.WriteLine("База данных уже содержит предметы. Seed предметов пропущен.");
            return;
        }

        var subjects = GetSubjectsToSeed();

        foreach (var subject in subjects)
        {
            await _subjectRepository.Create(subject);
            Console.WriteLine($"Создан предмет: {subject.Name.Value}");
        }

        Console.WriteLine($"Seed предметов завершён. Создано предметов: {subjects.Count}");
    }

    private async Task SeedAdminAsync()
    {
        // Пароль берется из переменной окружения ADMIN_PASSWORD, или "admin" по умолчанию
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? "admin"; 

        var existingAdmin = await _userRepository.GetByLoginAsync("admin");
        if (existingAdmin != null)
        {
            // Обновляем пароль, если пользователь уже существует (чтобы можно было сбросить)
            var newHash = PasswordHash.Create(_passwordHasher.Hash(adminPassword));
            existingAdmin.UpdatePassword(newHash);
            await _userRepository.UpdateAsync(existingAdmin);
            Console.WriteLine($"Пароль администратора (admin) обновлен.");
            return;
        }

        var login = Login.Create("admin");
        var passwordHash = PasswordHash.Create(_passwordHasher.Hash(adminPassword));
        var userId = UserId.New();
        
        var adminUser = new User(userId, login, passwordHash, UserRole.Admin);
        
        await _userRepository.AddAsync(adminUser);
        Console.WriteLine($"Создан пользователь-администратор: admin / {adminPassword}");
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
