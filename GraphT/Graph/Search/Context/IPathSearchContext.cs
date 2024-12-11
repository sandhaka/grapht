namespace GraphT.Graph.Search.Context;

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
    /// Retrieves the edges connected to a specified node in the graph, represented as a collection of EdgeTuple objects.
    /// </summary>
    /// <typeparam name="T">The type of the node values, which must implement the IEquatable interface.</typeparam>
    /// <param name="nodeValue">The value of the node for which the edges are to be retrieved.</param>
    /// <returns>A collection of EdgeTuple objects representing the edges connected to the specified node in the graph.</returns>
    IEnumerable<EdgeTuple<T>> NodeEdges(T nodeValue);

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