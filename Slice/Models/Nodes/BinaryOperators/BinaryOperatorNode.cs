using Slice.Models.Nodes;

namespace Slice.Models.Nodes.BinaryOperators;

public sealed class LeftRightChild
{
    public Node? LeftChild { get; set; }
    public Node? RightChild { get; set; }

    public LeftRightChild(Node? leftChild, Node? rightChild)
    {
        LeftChild = leftChild;
        RightChild = rightChild;
    }
}

public abstract class BinaryOperatorNode : Node<LeftRightChild>
{
    protected BinaryOperatorNode() : base(new LeftRightChild(null, null))
    { }
}