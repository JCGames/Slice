using Slice.Models.Nodes;

namespace Slice.Models.Nodes.UnaryOperators;

public abstract class UnaryOperatorNode() : Node<Node?>(null)
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + GetType().Name);
        Value?.Print(padding + '\t');
    }
}