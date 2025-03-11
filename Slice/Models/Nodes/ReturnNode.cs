namespace Slice.Models.Nodes;

public class ReturnNode(ExpressionNode expression) : Node
{
    public ExpressionNode Expression { get; set; } = expression;
    
    public override void Print(string padding)
    {
        Console.WriteLine(padding + nameof(ReturnNode));
        Expression.Print(padding + '\t');
    }
}