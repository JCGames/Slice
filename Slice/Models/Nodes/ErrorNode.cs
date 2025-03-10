namespace Slice.Models.Nodes;

public class ErrorNode : Node
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "Error");
    }
}