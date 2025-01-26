namespace Slice.Models.Nodes;

public class BlockNode : Node
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "{");
        foreach (var node in Children)
        {
            node.Print(padding + '\t');
        }
        Console.WriteLine(padding + "}");
    }
}