public record ResourceLocation
{
    public string Value { get; }

    public ResourceLocation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("File path cannot be empty");
        
        Value = value;
    }
}