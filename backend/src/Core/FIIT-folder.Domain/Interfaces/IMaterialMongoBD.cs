namespace FIIT_folder.Domain.Interfaces;

using FIIT_folder.Domain.Entities;

public interface IMaterialMongoDB
{
    Task<StudyMaterial?> GetByIdMaterial(Guid id);
    Task<List<StudyMaterial>> GetBySubjectId(Guid subjectId);
    Task<List<StudyMaterial>> GetAll();
    Task<StudyMaterial> Create(StudyMaterial material);
    Task<bool> Delete(Guid id);
    Task<StudyMaterial> GetByIdStudyMaterial(string id);
    Task<bool> DeleteStudyMaterial(string id);
    Task<StudyMaterial> CreateStudyMaterial(StudyMaterial material);
    
    Task<StudyMaterial> UpdateStudyMaterial(StudyMaterial material);
}