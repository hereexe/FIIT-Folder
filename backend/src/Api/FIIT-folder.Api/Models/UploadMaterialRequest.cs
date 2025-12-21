using Microsoft.AspNetCore.Http;

namespace FIIT_folder.Api.Models;

public class UploadMaterialRequest
{
    public Guid SubjectId { get; set; }
    public int Year { get; set; }
    public int Semester { get; set; }
    public string Description { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public IFormFile? File { get; set; }
}

public class MaterialResponse
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Semester { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public string MaterialType { get; set; } = string.Empty;
    public long Size { get; set; }
    public string SizeFormatted { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public string? CurrentUserRating { get; set; }
}