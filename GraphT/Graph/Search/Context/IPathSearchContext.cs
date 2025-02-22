using GraphT.Graph.Dto;

namespace GraphT.Graph.Search.Context;

/// <summary>
/// Represents the context required for path search operations within a graph.
/// </summary>
/// <typeparam name="TK">The type of node keys within the graph.</typeparam>
public interface IPathSearchContext<TK> where TK : IEquatable<TK>
{
    /// <summary>
    /// Gets the set of all unique node keys within the graph.
    /// </summary>
    /// <typeparam name="TK">The type of node keys.</typeparam>
    IReadOnlySet<TK> NodeKeys { get; }

    /// <summary>
    /// Retrieves the edges connected to a specified node in the graph, represented as a collection of EdgeTuple objects.
    /// </summary>
    /// <typeparam name="TK">The type of the node keys, which must implement the IEquatable interface.</typeparam>
    /// <param name="nodeKey">The key of the node for which the edges are to be retrieved.</param>
    /// <returns>A collection of EdgeTuple objects representing the edges connected to the specified node in the graph.</returns>
    IEnumerable<EdgeTuple<TK>> NodeEdges(TK nodeKey);

    /// <summary>
    /// Gets the starting node key for the path search within the graph.
    /// </summary>
    TK Start { get; }

    /// <summary>
    /// Gets the target node key for the search operation.
    /// The search algorithm attempts to find a path from the start node to this target node.
    /// </summary>
    /// <typeparam name="TK">The type of node keys within the graph.</typeparam>
    TK Target { get; }
}