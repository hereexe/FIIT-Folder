namespace FIIT_folder.Api.Models;

public class GetMaterialsRequest
{
    public Guid? SubjectId { get; set; }
    public string? MaterialType { get; set; }
}
