using System.Data;

namespace FIIT_folder.Domain.Value_Object;

public record SubjectSemester
{
    public int Value { get; }

    public SubjectSemester(int value)
    {
        if (value < 1 || value > 8)
            throw new ArgumentException("Value must be between 1 and 8", nameof(value));
        Value = value;
    }

}