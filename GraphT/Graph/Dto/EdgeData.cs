namespace GraphT.Graph.Dto;

/// <summary>
/// Represents an edge in a graph with a source node, destination node, and associated cost.
/// </summary>
/// <typeparam name="TK">The type of the nodes forming the edge, constrained to types implementing IEquatable.</typeparam>
/// <param name="From">The source node of the edge.</param>
/// <param name="Cost">The cost or weight of transitioning from the source node to the destination node.</param>
/// <param name="To">The destination node of the edge.</param>
public record EdgeData<TK>(TK From, decimal Cost, TK To) where TK : IEquatable<TK>
{
    public void Deconstruct(out TK from, out decimal cost, out TK to)
    {
        from = From;
        cost = Cost;
        to = To;
    }
}