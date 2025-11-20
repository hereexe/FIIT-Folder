namespace FIIT_folder.Api.Models;

public class CreateSubjectRequest
{
    public string Name { get; set; } = string.Empty;
    public int Semester { get; set; }
}