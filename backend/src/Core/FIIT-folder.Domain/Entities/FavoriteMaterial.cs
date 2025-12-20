using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Domain.Entities;

public class FavoriteMaterial
{
    public Guid Id { get; private set; }
    public UserId UserId { get; private set; } = default!;
    public StudyMaterialId MaterialId { get; private set; } = default!;
    public DateTime AddedAt { get; private set; }

    public FavoriteMaterial(UserId userId, StudyMaterialId materialId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        MaterialId = materialId;
        AddedAt = DateTime.UtcNow;
    }

    private FavoriteMaterial() { }
}
