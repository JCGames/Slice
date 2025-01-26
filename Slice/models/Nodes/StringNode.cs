namespace Slice.Models.Nodes;

public class StringNode : Node<string>
{
    protected StringNode(string value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}String: {Value}");
    }
}