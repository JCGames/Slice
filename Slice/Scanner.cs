using System.Text;

namespace Slice;

public sealed class Scanner : IDisposable
{
    private Stream _stream = null!;
    
    public string FilePath { get; private set; } = null!;
    public char Current { get; private set; } = '\0';
    public long Index => _stream.Position;
    public static bool IsEndOfLineCharacter(char c) => c is '\n' or '\r';

    public bool IsEndOfStream { get; private set; }
    public bool IsWhitespace => char.IsWhiteSpace(Current);
    public bool IsEndOfLine => IsEndOfLineCharacter(Current);
    public bool IsLetter => char.IsLetter(Current);
    public bool IsDigit => char.IsDigit(Current);
    public bool IsLetterOrDigit => char.IsLetterOrDigit(Current);
    public bool IsNumber => char.IsNumber(Current);

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
        Current = (char)_stream.ReadByte();
        IsEndOfStream = Current is '\0';   
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
        IsEndOfStream = false;
    }

    public void Dispose()
    {
        _stream.Dispose();
    }
}