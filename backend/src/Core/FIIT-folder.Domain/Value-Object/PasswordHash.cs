namespace FIIT_folder.Domain.Value_Object;

public record PasswordHash
{
    public string Value { get; }

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