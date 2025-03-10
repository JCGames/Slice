namespace Slice.Models.Nodes;

public class ExpressionNode : Node<Node?>
{
    public ExpressionNode() : base(null)
    { }
    
    public ExpressionNode(Node node) : base(node)
    { }
    
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "Expression");
        Value?.Print(padding + '\t');
    }
}