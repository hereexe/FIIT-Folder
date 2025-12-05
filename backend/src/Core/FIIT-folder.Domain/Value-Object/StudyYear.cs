namespace FIIT_folder.Domain.Value_Object;

public record StudyYear
{
    public int Value { get; }

    public StudyYear(int value)
    {
        if (value > 2018)
            throw new ArgumentException("Value must be greater than 2018");
        
        if (value > DateTime.UtcNow.Year + 2)
            throw new ArgumentException("A year cannot be more than 2 years into the future");
        
        Value = value;
    }
}