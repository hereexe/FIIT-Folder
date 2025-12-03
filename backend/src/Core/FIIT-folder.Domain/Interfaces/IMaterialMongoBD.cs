namespace FIIT_folder.Domain.Interfaces;

using FIIT_folder.Domain.Entities;

public interface IMaterialMongoDB
{
    Task<StudyMaterial> GetByIdMaterial(string id);
    Task<bool> DeleteMaterial(string id);
    Task<StudyMaterial> CreateMaterial(StudyMaterial material);
}