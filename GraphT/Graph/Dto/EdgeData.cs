namespace GraphT.Graph.Dto;

/// <summary>
/// Represents an edge in a graph with a source node, destination node, and associated cost.
/// </summary>
/// <typeparam name="T">The type of the nodes forming the edge, constrained to types implementing IEquatable.</typeparam>
/// <param name="From">The source node of the edge.</param>
/// <param name="Cost">The cost or weight of transitioning from the source node to the destination node.</param>
/// <param name="To">The destination node of the edge.</param>
public record EdgeData<T>(T From, decimal Cost, T To) where T : IEquatable<T>
{
    public void Deconstruct(out T from, out decimal cost, out T to)
    {
        from = From;
        cost = Cost;
        to = To;
    }
}