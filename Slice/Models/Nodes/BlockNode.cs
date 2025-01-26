namespace Slice.Models.Nodes;

public class BlockNode : Node<List<Node>>
{
    public BlockNode(List<Node> value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine(padding + "{");
        foreach (var node in Value)
        {
            node.Print(padding + '\t');
        }
        Console.WriteLine(padding + "}");
    }
}