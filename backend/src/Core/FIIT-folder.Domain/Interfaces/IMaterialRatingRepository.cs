using FIIT_folder.Domain.Entities;
using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Domain.Interfaces;

public interface IMaterialRatingRepository
{
    Task AddAsync(MaterialRating rating);
    Task UpdateAsync(MaterialRating rating);
    Task<MaterialRating?> GetByUserAndMaterialAsync(UserId userId, StudyMaterialId materialId);
    Task<List<MaterialRating>> GetByMaterialIdAsync(StudyMaterialId materialId);
    Task DeleteAsync(Guid materialId, Guid userId);
    Task<(int likes, int dislikes)> GetRatingCountsAsync(Guid materialId, CancellationToken
        cancellationToken = default);
}