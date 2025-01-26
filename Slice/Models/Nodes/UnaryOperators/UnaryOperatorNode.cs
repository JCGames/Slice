using Slice.Models.Nodes;

namespace Slice.Models.Nodes.UnaryOperators;

public abstract class UnaryOperatorNode : Node<Node?>
{
    protected UnaryOperatorNode() : base(null)
    { }
}