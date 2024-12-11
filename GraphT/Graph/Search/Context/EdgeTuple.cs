namespace GraphT.Graph.Search.Context;

public record EdgeTuple<T>
{
    /// <summary>
    /// Represents the value of a node.
    /// This property encapsulates the destination node associated with a specific edge.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the node value, which must implement <see cref="System.IEquatable{T}"/>.
    /// </typeparam>
    public T NodeValue { get; set; } = default!;

    /// <summary>
    /// Represents the cost associated with an edge in the graph.
    /// This property encapsulates the weighted value or distance of traversing an edge
    /// between nodes during graph traversal or pathfinding operations.
    /// </summary>
    public decimal Cost { get; set; } = decimal.Zero;
}