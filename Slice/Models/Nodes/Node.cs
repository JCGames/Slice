namespace Slice.Models.Nodes;

public abstract class Node
{
    public Meta Meta { get; set; } = new("", 0, 0, 0);

    public abstract void Print(string padding);
}

public abstract class Node<TValue>(TValue value) : Node
{
    public TValue Value { get; set; } = value;
}