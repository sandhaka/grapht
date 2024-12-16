using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph;

/// <summary>
/// Provides methods for modifying a graph structure in a mutable manner.
/// </summary>
/// <typeparam name="T">The type of the elements in the graph nodes. Must implement <see cref="IEquatable{T}"/>.</typeparam>
public interface IGraphMod<T> where T : IEquatable<T>
{
    /// <summary>
    /// Finalizes the modifications to the graph and returns an immutable representation of the graph.
    /// </summary>
    /// <returns>An immutable graph instance after applying all modifications.</returns>
    IGraph<T> EndMod();

    /// <summary>
    /// Adds a new node to the graph with optional inbound and outbound edges.
    /// </summary>
    /// <param name="value">The value of the node to be added.</param>
    /// <param name="inEdges">Optional inbound edges, represented as an array of <see cref="EdgeTuple{T}"/> instances.</param>
    /// <param name="outEdges">Optional outbound edges, represented as an array of <see cref="EdgeTuple{T}"/> instances.</param>
    /// <returns>An instance of <see cref="IGraphMod{T}"/> representing the modified graph.</returns>
    IGraphMod<T> AddNode(T value, Option<EdgeTuple<T>[]> inEdges, Option<EdgeTuple<T>[]> outEdges);

    /// <summary>
    /// Removes a node from the graph along with its associated edges.
    /// </summary>
    /// <param name="value">The value of the node to be removed.</param>
    /// <returns>An instance of <see cref="IGraphMod{T}"/> representing the modified graph after the node removal.</returns>
    IGraphMod<T> RemoveNode(T value);
}