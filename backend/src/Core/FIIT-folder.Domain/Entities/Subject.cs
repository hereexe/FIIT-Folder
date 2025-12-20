using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Domain.Entities;

public class Subject
{
    public SubjectId Id { get; private set; } = default!;
    public SubjectName Name { get; private set; } = default!;
    public SubjectSemester Semester { get; private set; } = default!;
    public IReadOnlyCollection<MaterialType> AvailableMaterialTypes => _materialTypes.AsReadOnly();
    
    private List<MaterialType> _materialTypes = new();

    public Subject(SubjectId id, SubjectName name, SubjectSemester semester, IEnumerable<MaterialType> materialTypes)
    {
        Id = id;
        Name = name;
        Semester = semester;
        _materialTypes = materialTypes?.Distinct().ToList() ?? new List<MaterialType>();
    }

    public void UpdateName(string newName)
    {
        Name = new SubjectName(newName);
    }

    public void SetMaterialTypes(IEnumerable<MaterialType> materialTypes)
    {
        var types = materialTypes?.ToList() ?? new List<MaterialType>();
        if (types.Count == 0)
            throw new ArgumentException("Предмет должен иметь хотя бы один тип материала");
        
        _materialTypes = types.Distinct().ToList();
    }

    public void AddMaterialType(MaterialType type)
    {
        if (!_materialTypes.Contains(type))
            _materialTypes.Add(type);
    }

    public void RemoveMaterialType(MaterialType type)
    {
        if (_materialTypes.Count <= 1)
            throw new InvalidOperationException("Нельзя удалить последний тип материала");
        
        _materialTypes.Remove(type);
    }

    public bool HasMaterialType(MaterialType type) => _materialTypes.Contains(type);

    private Subject() { }
}