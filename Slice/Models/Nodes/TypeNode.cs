namespace Slice.Models.Nodes;

public class TypeNode : Node<string>
{
    public TypeNode(string value) : base(value)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Type: {Value}");
    }
}