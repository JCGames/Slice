using Slice;
using Slice.Parser;

Diagnostics.ThrowInsteadOfExiting();

if (args.Length >= 1)
{
    var root = Parser
        .FromFile(args[0])
        .Parse();

    var analyzedTree = Parser.Analyze(root);
    analyzedTree.Print(string.Empty);
    
    return;
}

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
            
            var analyzedTree = Parser.Analyze(root);
            analyzedTree.Print(string.Empty);
        }
        catch (DiagnosticsException) { }
    }
}