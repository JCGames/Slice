using Slice.Models;

namespace Slice;

public static class Diagnostics
{
    private static bool _throwInsteadOfExiting;

    public static void ThrowInsteadOfExiting() => _throwInsteadOfExiting = true;
    
    public static void LogError(long line, long start, long end, string message)
    {
        var log = $"Fatal Error <{line}:{start}-{end}>: {message}";
        Console.WriteLine(log);

        if (_throwInsteadOfExiting) throw new DiagnosticsException(log);
        Environment.Exit(1);
    }

    public static void LogError(Meta meta, string message) => LogError(meta.Line, meta.Start, meta.End, message);
    
    public static void LogWarning(string message)
    {
        
    }
    
    public static void LogInformation(string message)
    {
        
    }
}

public class DiagnosticsException(string message) : Exception(message) 
{ }