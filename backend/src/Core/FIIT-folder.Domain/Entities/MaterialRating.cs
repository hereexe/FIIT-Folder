using FIIT_folder.Domain.Value_Object;
using FIIT_folder.Domain.Enums;

namespace FIIT_folder.Domain.Entities;

public class MaterialRating
{
    public Guid Id { get; set; }
    public StudyMaterialId MaterialId { get; set; }
    public UserId UserId { get; set; }
    public RatingType Rating { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    
}