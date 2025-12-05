namespace FIIT_folder.Domain.Value_Object;

public record MaterialName
{
    private const int MaxLength = 200;

    public string Value { get; }

    public MaterialName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
            throw new ArgumentException($"'{nameof(value)}' cannot be longer than {MaxLength}.", nameof(value));
        Value = trimmed;
    }

    public override string ToString() => Value;
}