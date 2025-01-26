namespace Slice.Models.Nodes;

public class IdentifierNode(string value) : Node<string>(value)
{
    public override void Print(string padding)
    {
        Console.WriteLine($"{padding}Identifier: {Value}");
    }
}