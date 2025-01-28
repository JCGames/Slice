namespace Slice.Models.Nodes.BinaryOperators;

public class DeclarationNode : BinaryOperatorNode
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + nameof(DeclarationNode)[..^4]);
        Console.WriteLine(padding + "LEFT:");
        Value.LeftChild?.Print(padding + '\t');
        Console.WriteLine(padding + "RIGHT:");
        Value.RightChild?.Print(padding + '\t');
    }
}