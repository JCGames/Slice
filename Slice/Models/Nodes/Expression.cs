namespace Slice.Models.Nodes;

public class Expression : Node<Node?>
{
    public Expression() : base(null)
    { }

    public override void Print(string padding)
    {
        Console.WriteLine(padding + "Expression");
        Value?.Print(padding + '\t');
    }
}