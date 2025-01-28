namespace Slice.Models.Nodes.BinaryOperators;

public class LessThanNode : BinaryOperatorNode
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + nameof(LessThanNode)[..^4]);
        Console.WriteLine(padding + "LEFT:");
        Value.LeftChild?.Print(padding + '\t');
        Console.WriteLine(padding + "RIGHT:");
        Value.RightChild?.Print(padding + '\t');
    }
}