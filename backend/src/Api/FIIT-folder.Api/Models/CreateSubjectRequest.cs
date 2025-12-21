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
    public List<MaterialTypeResponse> MaterialTypes { get; set; } = new();
}

public class MaterialTypeResponse
{
    public string Value { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public class SubjectWithMaterialsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MaterialGroupResponse> Content { get; set; } = new();
}

public class MaterialGroupResponse
{
    public string ExamType { get; set; } = string.Empty;
    public string RawType { get; set; } = string.Empty;
    public List<MaterialGroupItemResponse> Items { get; set; } = new();
}

public class MaterialGroupItemResponse
{
    public string DisplayName { get; set; } = string.Empty;
    public int Semester { get; set; }
    public Guid SubjectId { get; set; }
}