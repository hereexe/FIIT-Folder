using FIIT_folder.Domain.Interfaces;
using FIIT_folder.Domain.Models;

namespace FIIT_folder.Infrastructure.FileStorage;

public class MaterialMongoDB : IMaterialMongoDB
{
    
    public Task<Material> GetByIdMaterial(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteMaterial(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Material> CreateMaterial(Material material)
    {
        throw new NotImplementedException();
    }
}