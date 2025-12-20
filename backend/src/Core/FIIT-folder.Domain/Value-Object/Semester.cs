namespace FIIT_folder.Domain.Value_Object;

public record Semester
{
    public int Value { get; }

    public Semester(int value)
    {
        if (value < 1 || value > 8)
            throw new ArgumentException("Value must be between 1 and 8");
        
        Value = value;
    }
}