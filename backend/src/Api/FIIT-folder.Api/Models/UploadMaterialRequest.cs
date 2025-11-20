using Microsoft.AspNetCore.Http;

namespace FIIT_folder.Api.Models;

public class UploadMaterialRequest
{
    public string Subject { get; set; } = string.Empty;
    public IFormFile? File { get; set; }
}