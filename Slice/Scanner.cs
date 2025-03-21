using System.Text;

namespace Slice;

public sealed class Scanner : IDisposable
{
    private Stream _stream = null!;
    
    public string FilePath { get; private set; } = null!;
    public char Current { get; private set; } = '\0';
    public long Index => _stream.Position;
    public bool IsEndOfStream => _stream.Position == _stream.Length && (Current == 0 || Current == 65535);

    private Scanner() { }
    
    public static Scanner FromFile(string filePath)
    {
        var scanner = new Scanner
        {
            _stream = new FileStream(filePath, FileMode.Open),
            FilePath = Path.GetFileName(filePath)
        };

        scanner.Current = (char)scanner._stream.ReadByte();
        return scanner;
    }

    public static Scanner FromText(string fileName, string text)
    {
        var scanner = new Scanner
        {
            _stream = new MemoryStream(Encoding.UTF8.GetBytes(text + '\0')),
            FilePath = fileName
        };
        
        scanner.Current = (char)scanner._stream.ReadByte();
        return scanner;
    }

    public void Next()
    {
        if (IsEndOfStream) return;
        
        Current = (char)_stream.ReadByte();
    }

    public char Peek()
    {
        var c = (char)_stream.ReadByte();
        _stream.Seek(-1, SeekOrigin.Current);
        return c;
    }
    
    public void Back()
    {
        if (Index <= 1) return;
        
        // go back
        _stream.Seek(-2, SeekOrigin.Current);
        
        // read the byte here
        Current = (char)_stream.ReadByte();
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}