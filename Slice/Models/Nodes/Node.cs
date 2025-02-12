namespace Slice.Models.Nodes;

public abstract class Node
{
    public Meta Meta { get; set; }

    public abstract void Print(string padding);
}

public abstract class Node<TValue> : Node
{
    public TValue Value { get; set; }
    
    protected Node(TValue value)
    {
        Value = value;
    }
}