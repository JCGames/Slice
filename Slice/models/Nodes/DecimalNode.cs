namespace Slice.Models.Nodes;

public class DecimalNode : Node<double>
{
    protected DecimalNode(double value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Decimal: {Value}");
    }
}