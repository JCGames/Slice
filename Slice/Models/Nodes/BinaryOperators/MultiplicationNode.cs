namespace Slice.Models.Nodes.BinaryOperators;

public class MultiplicationNode : BinaryOperatorNode
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "Multiplication");
        Console.WriteLine(padding + "LEFT:");
        Value.LeftChild?.Print(padding + '\t');
        Console.WriteLine(padding + "RIGHT:");
        Value.RightChild?.Print(padding + '\t');
    }
}