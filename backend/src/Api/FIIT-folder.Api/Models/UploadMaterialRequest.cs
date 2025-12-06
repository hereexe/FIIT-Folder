using Microsoft.AspNetCore.Http;

namespace FIIT_folder.Api.Models;

public class UploadMaterialRequest
{
    public Guid SubjectId { get; set; }
    public int Year { get; set; }
    public string MaterialType { get; set; } = string.Empty;
    public IFormFile? File { get; set; }
}

public class MaterialResponse
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public string MaterialType { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}