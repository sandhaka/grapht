namespace GraphT.Graph.Exceptions;

public sealed class InvalidGraphDataException<T> : Exception
{
    public InvalidGraphDataException() { }

    public InvalidGraphDataException(T item) : base($"Invalid graph data: {item}")
    {
        Item = item;
    }
    
    public T? Item { get; }
}