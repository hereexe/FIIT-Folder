using FIIT_folder.Domain.Entities;

namespace FIIT_folder.Domain.Interfaces;

public interface ISubjectRepository
{
    Task<Subject?> GetById(Guid id);
    Task<List<Subject>> GetAll();
    Task<Subject> Create(Subject subject);
    Task<bool> Update(Subject subject);
    Task<bool> Delete(Guid id);
    Task<List<Subject>> GetByName(string name);
    Task<bool> ExistsByName(string name);
}