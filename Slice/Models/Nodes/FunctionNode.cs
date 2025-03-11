namespace Slice.Models.Nodes;

public class FunctionNode(IdentifierNode identifier, TypeNode? type, BlockNode body) : Node
{
    public IdentifierNode Identifier { get; init; } = identifier;
    public List<ParameterNode> Parameters { get; set; } = [];
    public TypeNode? ReturnType { get; set; } = type;
    public BlockNode Body { get; set; } = body;
    
    public override void Print(string padding)
    {
        Console.WriteLine(padding + nameof(FunctionNode));
        Console.WriteLine(padding + $"Name: {Identifier.Value}");
        Console.WriteLine(padding + "Parameters: [");
        Parameters.ForEach(x => x.Print(padding + '\t'));
        Console.WriteLine(padding + "]");
        Console.WriteLine(padding + $"ReturnType: {ReturnType?.Value}");
        Console.WriteLine(padding + "Body:");
        Body.Print(padding);
    }
}