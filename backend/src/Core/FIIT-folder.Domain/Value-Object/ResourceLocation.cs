public record ResourceLocation
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf",
        ".doc", ".docx",
        ".xls", ".xlsx",
        ".png", ".jpg", ".jpeg",
        ".ppt", ".pptx",
        ".txt"
    };
    
    public string Value { get; }

    public ResourceLocation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("File path cannot be empty");
        
        var extension = Path.GetExtension(value);
        if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
            throw new ArgumentException($"You can only: {string.Join(", ", AllowedExtensions)}");
        
        Value = value;
    }
    
    public static bool IsAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return !string.IsNullOrEmpty(extension) && AllowedExtensions.Contains(extension);
    }
}