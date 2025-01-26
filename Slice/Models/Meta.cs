namespace Slice.Models;

public sealed class Meta
{
    public string FilePath { get; set; }
    public long Line { get; set; }
    public long Start { get; set; }
    public long End { get; set; }

    public Meta(string filePath, long line, long start, long end)
    {
        FilePath = filePath;
        Line = line;
        Start = start;
        End = end;
    }
}