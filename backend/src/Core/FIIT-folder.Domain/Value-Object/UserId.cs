namespace FIIT_folder.Domain.Value_Object;

public record UserId
{
    public Guid Value { get; private set; }

    // Private parameterless constructor for MongoDB deserialization
    private UserId()
    {
        Value = Guid.Empty;
    }

    public UserId(Guid value)
    {
        if (value == Guid.Empty) throw new ArgumentException("Value cannot be empty", nameof(value));
        Value = value;
    }
    
    public static UserId New() => new UserId(Guid.NewGuid());
    
    public override string ToString() => Value.ToString();
}