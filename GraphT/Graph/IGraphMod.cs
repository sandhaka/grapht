using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph;

/// <summary>
/// Provides methods for modifying a graph structure in a mutable manner.
/// </summary>
/// <typeparam name="TK">The key type of the elements in the graph nodes. Must implement <see cref="IEquatable{TK}"/>.</typeparam>
public interface IGraphMod<TK> where TK : IEquatable<TK>
{
    /// <summary>
    /// Finalizes the modifications to the graph and returns an immutable representation of the graph.
    /// </summary>
    /// <returns>An immutable graph instance after applying all modifications.</returns>
    IGraph<TK> EndMod();

    /// <summary>
    /// Adds a new node to the graph with optional inbound and outbound edges.
    /// </summary>
    /// <param name="key">The key of the node to be added.</param>
    /// <param name="inEdges">Optional inbound edges, represented as an array of <see cref="EdgeTuple{TK}"/> instances.</param>
    /// <param name="outEdges">Optional outbound edges, represented as an array of <see cref="EdgeTuple{TK}"/> instances.</param>
    /// <returns>An instance of <see cref="IGraphMod{TK}"/> representing the modified graph.</returns>
    IGraphMod<TK> AddNode(TK key, Option<EdgeTuple<TK>[]> inEdges, Option<EdgeTuple<TK>[]> outEdges);

    /// <summary>
    /// Removes a node from the graph along with its associated edges.
    /// </summary>
    /// <param name="key">The key of the node to be removed.</param>
    /// <returns>An instance of <see cref="IGraphMod{TK}"/> representing the modified graph after the node removal.</returns>
    IGraphMod<TK> RemoveNode(TK key);
}