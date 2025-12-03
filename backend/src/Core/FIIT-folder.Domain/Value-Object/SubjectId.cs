namespace FIIT_folder.Domain.Value_Object;

public record SubjectId(Guid Value)
{
    public static SubjectId New() => new SubjectId(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}