using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FIIT_folder.Domain.Value_Object;

public record Login
{
    public string Value { get; }
    private static readonly Regex AllowedPattern = new(@"^[a-zA-Z0-9_]+$", RegexOptions.Compiled);

    private Login(string value)
    {
        Value = value;
    }

    public static Login Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        
        var trimmedValue = value.Trim().ToLowerInvariant();
        
        if (trimmedValue.Length < 4 || trimmedValue.Length > 20)
            throw new ArgumentException("Value must be between 4 and 20 characters.", nameof(value));
        
        if (!AllowedPattern.IsMatch(trimmedValue))
            throw new ArgumentException("Value is not allowed.", nameof(value));
        
        return new Login(trimmedValue);
    }
}