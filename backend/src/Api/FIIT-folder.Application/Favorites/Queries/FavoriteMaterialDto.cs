namespace FIIT_folder.Application.Favorites.Queries;

public class FavoriteMaterialDto
{
    public Guid Id { get; set; }
    public Guid MaterialId { get; set; }
    public string? Name { get; set; } 
    public string? FilePath { get; set; }
    public string? MaterialType { get; set; }
    public long? Size { get; set; }
    public int? Year { get; set; }
    public int? Semester { get; set; }
    public string? Description { get; set; }
    public string? AuthorName { get; set; }
    public Guid? SubjectId { get; set; }
    public DateTime AddedAt { get; set; }
}
