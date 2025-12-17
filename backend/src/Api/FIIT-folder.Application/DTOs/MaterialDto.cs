namespace FIIT_folder.Application.DTOs;

public class MaterialDto
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
    public string FilePath { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
