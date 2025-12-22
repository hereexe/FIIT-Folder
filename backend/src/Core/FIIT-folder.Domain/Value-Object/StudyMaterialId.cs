namespace FIIT_folder.Domain.Value_Object;

public record MaterialId(Guid Value)
{
    public static MaterialId New() => new MaterialId(Guid.NewGuid());
}