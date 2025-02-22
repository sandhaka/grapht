namespace GraphT.Graph.Exceptions;

public sealed class InvalidGraphDataException<TK> : Exception
{
    public InvalidGraphDataException() { }

    public InvalidGraphDataException(TK item) : base($"Invalid graph data: {item}")
    {
        Item = item;
    }
    
    public TK? Item { get; }
}