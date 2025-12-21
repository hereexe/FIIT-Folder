namespace FIIT_folder.Application.DTOs;

public class SubjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Semester { get; set; }
    public List<MaterialTypeDto> MaterialTypes { get; set; } = new();
}

public class MaterialTypeDto
{
    public string Value { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public class SubjectWithMaterialsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MaterialGroupDto> MaterialGroups { get; set; } = new();
}

public class MaterialGroupDto
{
    public string ExamType { get; set; } = string.Empty;
    public string RawType { get; set; } = string.Empty;
    public List<MaterialGroupItemDto> Items { get; set; } = new();
}

public class MaterialGroupItemDto
{
    public string DisplayName { get; set; } = string.Empty;
    public int Semester { get; set; }
    public Guid SubjectId { get; set; }
}
