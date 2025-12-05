namespace FIIT_folder.Domain.Value_Object;

public record MaterialSize
{
    public long Size { get; }

    public MaterialSize(long size)
    {
        if (size < 0)
            throw new ArgumentOutOfRangeException(nameof(size));
        if (size > 10 * 1024 * 1024)
            throw new ArgumentOutOfRangeException(nameof(size));
        
        Size = size;
    }

    private double ToKilobytes() => Size / 1024.0;
    private double ToMegabytes() => Size / (1024.0 * 1024);

    public override string ToString() => Size switch
    {
        < 1024 => $"{Size} B",
        < 1024 * 1024 => $"{ToKilobytes():F2} KB",
        _ => $"{ToMegabytes():F2} MB"
    };
}