namespace Slice.Models.Nodes;

public class IdentifierNode : Node<string>
{
    public IdentifierNode(string value) : base(value) 
    { }

    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Identifier: {Value}");
    }
}