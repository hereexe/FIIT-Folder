namespace FIIT_folder.Domain.Interfaces;

using FIIT_folder.Domain.Models;
using FIIT_folder.Domain.Entities;

public interface IMaterialMongoDB
{
    Task<StudyMaterial> GetByIdStudyMaterial(string id);
    Task<bool> DeleteStudyMaterial(string id);
    Task<StudyMaterial> CreateStudyMaterial(StudyMaterial material);
    
    Task<StudyMaterial> UpdateStudyMaterial(StudyMaterial material);
}