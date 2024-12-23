namespace GraphT.Graph.Exceptions;

public sealed class InvalidGraphDataException<T> : Exception
{
    public required T Item { get; init; }

    public override string Message => $"Invalid graph data for: {Item} item";
}