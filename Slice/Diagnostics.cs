namespace Slice;

public static class Diagnostics
{
    private static bool _disableExiting;
    
    public static void DisableExiting()
    {
        _disableExiting = true;
    }
    
    public static void LogError(string message)
    {
        Console.WriteLine(message);

        if (_disableExiting) throw new DiagnosticsException($"Exited due to error: {message}");
        
        Environment.Exit(1);
    }
    
    public static void LogWarning(string message)
    {
        
    }
    
    public static void LogInformation(string message)
    {
        
    }
}

public class DiagnosticsException(string message) : Exception(message) 
{ }