namespace FIIT_folder.Domain.Value_Object;

public record StudyMaterialId(Guid Value)
{
    public static StudyMaterialId New() => new StudyMaterialId(Guid.NewGuid());
}