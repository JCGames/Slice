namespace Slice.Models.Nodes;

public class AssignmentNode : Node
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "ASSIGNMENT");
        Console.WriteLine(padding + "LEFT:");
        Children[0].Print(padding + '\t');
        Console.WriteLine(padding + "RIGHT:");
        Children[1].Print(padding + '\t');
    }
}