namespace Slice.Models.Nodes.BinaryOperators;

public sealed class LeftRightChild(Node? leftChild, Node? rightChild)
{
    public Node? LeftChild { get; set; } = leftChild;
    public Node? RightChild { get; set; } = rightChild;
}

public abstract class BinaryOperatorNode() : Node<LeftRightChild>(new LeftRightChild(null, null))
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + GetType().Name);
        Console.WriteLine(padding + "Left:");
        Value.LeftChild?.Print(padding + '\t');
        Console.WriteLine(padding + "Right:");
        Value.RightChild?.Print(padding + '\t');
    }
}