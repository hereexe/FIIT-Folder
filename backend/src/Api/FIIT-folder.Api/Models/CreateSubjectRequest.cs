namespace FIIT_folder.Api.Models;

public class CreateSubjectRequest
{
    public string Name { get; set; } = string.Empty;
    public int Semester { get; set; }
    public List<string> MaterialTypes { get; set; } = new();
}

public class SubjectResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Semester { get; set; }
    public List<string> MaterialTypes { get; set; } = new();
}