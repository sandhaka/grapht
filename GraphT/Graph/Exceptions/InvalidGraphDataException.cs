using System.Diagnostics.CodeAnalysis;

namespace GraphT.Graph.Exceptions;

public sealed class InvalidGraphDataException<T> : Exception
{
    [SetsRequiredMembers]
    public InvalidGraphDataException(T item) : base($"Invalid graph data: {item}")
    {
        Item = item;
    }
    
    public required T Item { get; init; }
}