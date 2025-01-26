namespace Slice.Models.Nodes;

public class Expression() : Node<Node?>(null)
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "Expression");
        Value?.Print(padding + '\t');
    }
}