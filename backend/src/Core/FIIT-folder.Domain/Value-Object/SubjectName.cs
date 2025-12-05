namespace FIIT_folder.Domain.Value_Object;

public record SubjectName
{
    public string Value { get; }

    public SubjectName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        
        Value = value.Trim();
    }
}