namespace Slice.Models.Nodes.BinaryOperators;

public class AdditionNode : BinaryOperatorNode
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "Addition");
        Console.WriteLine(padding + "LEFT:");
        Value.LeftChild?.Print(padding + '\t');
        Console.WriteLine(padding + "RIGHT:");
        Value.RightChild?.Print(padding + '\t');
    }
}