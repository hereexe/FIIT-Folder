namespace FIIT_folder.Domain.Entities;

"rrrrr"

public class StudyMaterial
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Guid SubjectId { get; private set; }
    public Guid UserId { get; private set; }
    public int Year { get; private set; }
    public MaterialType MaterialType { get; private set; }
    public string FileReference { get; private set; } = string.Empty;
    public DateTime UploadedAt { get; private set; }

    public StudyMaterial(string name, Guid subjectId, Guid userId, int year, MaterialType materialType,
        string fileReference)
    {
        Id = Guid.NewGuid();
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("name cannot be empty");
        Name = name;
        
        if (year < 2018)
            throw new ArgumentOutOfRangeException("year must be greater than 2018");
        Year = year;
        
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty");
        UserId = userId;
        
        if (subjectId == Guid.Empty)
            throw new ArgumentException("SubjectId cannot be empty");
        SubjectId = subjectId;
        
        MaterialType = materialType;
        FileReference = fileReference ?? throw new ArgumentNullException(nameof(fileReference));
        UploadedAt = DateTime.UtcNow;
    }
    
    private StudyMaterial() { }
}