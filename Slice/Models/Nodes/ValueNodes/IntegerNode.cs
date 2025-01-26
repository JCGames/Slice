using Slice.Models.Nodes;

namespace Slice.Models.Nodes.ValueNodes;

public class IntegerNode : Node<int>
{
    public IntegerNode(int value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Integer: {Value}");
    }
}