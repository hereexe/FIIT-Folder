namespace FIIT_folder.Domain.Interfaces;

using FIIT_folder.Domain.Entities;

public interface IMaterialMongoDB
{
    Task<StudyMaterial?> GetByIdMaterial(Guid id);
    Task<List<StudyMaterial>> GetBySubjectId(Guid subjectId);
    Task<List<StudyMaterial>> GetAll();
    Task<StudyMaterial> Create(StudyMaterial material);
    Task<bool> Delete(Guid id);
}