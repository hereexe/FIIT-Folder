namespace FIIT_folder.Domain.Value_Object;

public record PasswordHash
{
    public string Value { get; private set; }

    // Private parameterless constructor for MongoDB deserialization
    private PasswordHash() 
    {
        Value = string.Empty;
    }

    public PasswordHash(string hash)
    {
        Value = hash;
    }

    public static PasswordHash Create(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Хеш пароля не может быть пустым");

        return new PasswordHash(hash);
    }

    public override string ToString() => "***";
}