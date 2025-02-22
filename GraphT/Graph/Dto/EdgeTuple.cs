namespace GraphT.Graph.Dto;

public record EdgeTuple<TK>
{
    /// <summary>
    /// Represents the unique identifier or key associated with a node in a graph structure.
    /// </summary>
    public TK NodeKey { get; set; } = default!;

    /// <summary>
    /// Represents the cost associated with an edge in the graph.
    /// This property encapsulates the weighted value or distance of traversing an edge
    /// between nodes during graph traversal or pathfinding operations.
    /// </summary>
    public decimal Cost { get; set; } = decimal.Zero;

    public void Deconstruct(out TK nodeKey, out decimal cost)
    {
        nodeKey = NodeKey;
        cost = Cost;
    }
}