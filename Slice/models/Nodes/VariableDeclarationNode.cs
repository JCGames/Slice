namespace Slice.Models.Nodes;

public class VariableDeclarationNode : Node
{
    public override void Print(string padding)
    {
        Console.WriteLine(padding + "VARIABLE DECLARATION");
        Console.WriteLine(padding + "LEFT:");
        Children[0].Print(padding + '\t');
        Console.WriteLine(padding + "RIGHT:");
        Children[1].Print(padding + '\t');
    }
}