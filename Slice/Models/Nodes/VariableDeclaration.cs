namespace Slice.Models.Nodes;

public class VariableDeclaration(TypeNode type, IdentifierNode identifier) : Node
{
    public TypeNode Type { get; init; } = type;
    public IdentifierNode Name { get; init; } = identifier;
    
    public override void Print(string padding)
    {
        Console.WriteLine(padding + nameof(VariableDeclaration));
        Console.WriteLine(padding + $"Type: {Type.Value}, Name: {Name.Value}");
    }
}