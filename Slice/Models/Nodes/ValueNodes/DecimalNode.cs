using Slice.Models.Nodes;

namespace Slice.Models.Nodes.ValueNodes;

public class DecimalNode : Node<double>
{
    public DecimalNode(double value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Decimal: {Value}");
    }
}