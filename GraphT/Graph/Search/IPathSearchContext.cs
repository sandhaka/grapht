namespace GraphT.Graph.Search;

/// <summary>
/// Represents the context required for path search operations within a graph.
/// </summary>
/// <typeparam name="T">The type of node values within the graph.</typeparam>
public interface IPathSearchContext<T> where T : IEquatable<T>
{
    /// <summary>
    /// Gets the set of all unique node values within the graph.
    /// </summary>
    /// <typeparam name="T">The type of node values.</typeparam>
    IReadOnlySet<T> NodeValues { get; }

    /// <summary>
    /// Iterates over the neighbors of the specified node, applying the specified action to each neighbor.
    /// </summary>
    /// <param name="nodeValue">The value of the node whose neighbors will be iterated over.</param>
    /// <param name="body">The action to apply to each neighboring node and its associated cost.</param>
    void ForeachNeighbors(T nodeValue, Action<T, decimal> body);

    /// <summary>
    /// Gets the starting node value for the path search within the graph.
    /// </summary>
    T Start { get; }

    /// <summary>
    /// Gets the target node value for the search operation.
    /// The search algorithm attempts to find a path from the start node to this target node.
    /// </summary>
    /// <typeparam name="T">The type of node values within the graph.</typeparam>
    T Target { get; }
}