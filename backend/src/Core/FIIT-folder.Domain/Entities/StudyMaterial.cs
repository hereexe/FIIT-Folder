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
    public ResourceLocation FileReference { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public StudyMaterial(MaterialName name, SubjectId subjectId, UserId userId, StudyYear year, MaterialType materialType,
        ResourceLocation fileReference)
    {
        Id = StudyMaterialId.New();
        Name = name;
        SubjectId = subjectId;
        UserId = userId;
        Year = year;       
        MaterialType = materialType;
        FileReference = fileReference;
        
        UploadedAt = DateTime.UtcNow;
    }
    
    private StudyMaterial() { }
}