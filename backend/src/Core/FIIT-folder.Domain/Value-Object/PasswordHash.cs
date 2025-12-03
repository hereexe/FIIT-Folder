namespace FIIT_folder.Domain.Value_Object;

public record PasswordHash
{
    public string Value { get; }

    public PasswordHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash)) throw new ArgumentNullException(nameof(hash));
        Value = hash;
    }
}