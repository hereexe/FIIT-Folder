namespace FIIT_folder.Domain.Interfaces;

using FIIT_folder.Domain.Entities;

public interface IMaterialMongoDB
{
    Task<List<Material>> GetBySubjectId(Guid subjectId);
    Task<List<Material>> GetAll();
    Task<Material> GetByNameMaterial(string nameMaterial);
    Task<bool> DeleteMaterial(Guid id);
    Task<Material> GetByIdMaterial(Guid id);
    Task<Material> CreateMaterial(Material material);
    
    Task<Material> UpdateMaterial(Material material);
}