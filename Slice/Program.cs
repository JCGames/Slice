using Slice;

Diagnostics.ThrowInsteadOfExiting();

while (true)
{
    Console.Write(":> ");
    var command = Console.ReadLine();

    if (command is "exit") break;

    if (!string.IsNullOrWhiteSpace(command))
    {
        try 
        {
            var root = Parser
                .FromText("cmdline", command)
                .Parse();

            root?.Print(string.Empty);
        }
        catch (DiagnosticsException) { }
    }
}