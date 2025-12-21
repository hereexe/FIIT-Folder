namespace FIIT_folder.Domain.Interfaces;

using FIIT_folder.Domain.Entities;

public interface IMaterialMongoDB
{
    Task<List<StudyMaterial>> GetBySubjectId(Guid subjectId);
    Task<List<StudyMaterial>> GetAll();
    Task<StudyMaterial?> GetByNameStudyMaterial(string nameStudyMaterial);
    Task<bool> DeleteStudyMaterial(Guid id);
    Task<StudyMaterial?> GetByIdStudyMaterial(Guid id);
    Task<StudyMaterial> CreateStudyMaterial(StudyMaterial material);
    
    Task<StudyMaterial?> UpdateStudyMaterial(StudyMaterial material);
    Task<List<StudyMaterial>> SearchAsync(string searchText);
}