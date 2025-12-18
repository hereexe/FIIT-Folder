namespace FIIT_folder.Api.Models;

public class GetMaterialsRequest
{
    public Guid? SubjectId { get; set; }
    public string? MaterialType { get; set; }
    public int? Semester { get; set; }
    public int? Year { get; set; }
    public string? SearchText { get; set; }
}
