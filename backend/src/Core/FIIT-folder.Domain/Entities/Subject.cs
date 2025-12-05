using FIIT_folder.Domain.Value_Object;

namespace FIIT_folder.Domain.Entities;

public class Subject
{
    public SubjectId Id {get; private set;}
    public SubjectName Name {get; private set;} 

    public Subject(SubjectId id, SubjectName name)
    {
        Id = id;
        Name = name;
    }

    public void UpdateName(string newName)
    {
        Name = new SubjectName(newName);
    }

    private Subject() { }
}