namespace FIIT_folder.Domain.Interfaces;

using FIIT_folder.Domain.Models;

public interface IMaterialMongoDB
{
    Task<Material> GetByIdMaterial(string id);
    Task<bool> DeleteMaterial(string id);
    Task<Material> CreateMaterial(Material material);
}