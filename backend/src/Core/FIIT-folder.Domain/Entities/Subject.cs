namespace FIIT_folder.Domain.Entities;

public class Subject
{
    public Guid Id {get; private set;}
    public string Name {get; private set;}

    public Subject(string name)
    {
        Id = Guid.NewGuid();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("Subject cannot be empty");
        Name = name;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentNullException("Name subject cannot be empty");
        Name = newName;
    }

    private Subject() { }
}