namespace Slice.Models.Nodes;

public class IntegerNode : Node<int>
{
    protected IntegerNode(int value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Integer: {Value}");
    }
}