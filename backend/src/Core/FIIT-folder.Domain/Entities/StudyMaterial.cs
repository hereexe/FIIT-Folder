using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Domain.Entities;

public class StudyMaterial
{
    public StudyMaterialId Id { get; private set; }
    public SubjectId  SubjectId { get; private set; }
    public UserId UserId { get; private set; }
    public MaterialName Name { get; private set; }
    public StudyYear Year { get; private set; }
    public Semester Semester { get; private set; }
    public string Description { get; private set; }
    public MaterialType MaterialType { get; private set; }
    public MaterialSize Size { get; private set; }
    public ResourceLocation FilePath { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public StudyMaterial(MaterialName name, SubjectId subjectId, UserId userId, StudyYear year, Semester semester, string description, MaterialSize size, MaterialType materialType,
        ResourceLocation fileReference)
    {
        Id = StudyMaterialId.New();
        Name = name;
        SubjectId = subjectId;
        UserId = userId;
        Year = year;
        Semester = semester;
        Description = description;
        Size = size;
        MaterialType = materialType;
        FilePath = fileReference;
        UploadedAt = DateTime.UtcNow;
    }
    
    private StudyMaterial() { }
}