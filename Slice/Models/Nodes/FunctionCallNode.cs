namespace Slice.Models.Nodes;

public class FunctionCallNode(IdentifierNode identifierNode) : Node
{
    public IdentifierNode Identifier { get; set; } = identifierNode;
    public List<ExpressionNode> Arguments { get; set; } = [];

    public override void Print(string padding)
    {
        Console.WriteLine(padding + nameof(FunctionCallNode));
        Console.WriteLine(padding + $"Name: {Identifier.Value}");
        Console.WriteLine(padding + "Arguments: [");
        Arguments.ForEach(a => a.Print(padding + '\t'));
        Console.WriteLine(padding + "]");
    }
}