namespace Slice.Models.Nodes;

public class ParameterNode(IdentifierNode name, TypeNode type) : Node
{
    public IdentifierNode Name { get; init; } = name;
    public TypeNode Type { get; init; } = type;
    
    public override void Print(string padding)
    {
        Console.WriteLine(padding + nameof(ParameterNode));
        Console.WriteLine(padding + $"Name: {Name.Value}, Type: {Type.Value}");
    }
}