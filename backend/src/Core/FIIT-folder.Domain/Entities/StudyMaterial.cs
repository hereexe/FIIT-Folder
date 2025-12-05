using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Domain.Entities;

public class StudyMaterial
{
    public StudyMaterialId Id { get; private set; }
    public SubjectId  SubjectId { get; private set; }
    public UserId UserId { get; private set; }
    public MaterialName Name { get; private set; }
    public StudyYear Year { get; private set; }
    public MaterialType MaterialType { get; private set; }
    public MaterialSize Size { get; private set; }
    public ResourceLocation FilePath { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public StudyMaterial(MaterialName name, SubjectId subjectId, UserId userId, StudyYear year,MaterialSize size, MaterialType materialType,
        ResourceLocation fileReference)
    {
        Id = StudyMaterialId.New();
        Name = name;
        SubjectId = subjectId;
        UserId = userId;
        Year = year;       
        Size = size;
        MaterialType = materialType;
        FilePath = fileReference;
        UploadedAt = DateTime.UtcNow;
    }
    
    private StudyMaterial() { }
}