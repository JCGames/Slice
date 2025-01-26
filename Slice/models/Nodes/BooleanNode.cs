namespace Slice.Models.Nodes;

public class BooleanNode : Node<bool>
{
    protected BooleanNode(bool value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Boolean: {Value}");
    }
}