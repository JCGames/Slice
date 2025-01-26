using Slice.Models.Nodes;

namespace Slice.Models.Nodes.ValueNodes;

public class BooleanNode : Node<bool>
{
    public BooleanNode(bool value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Boolean: {Value}");
    }
}