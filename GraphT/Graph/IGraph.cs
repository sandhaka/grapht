using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph;

/// <summary>
/// Represents a generic graph interface.
/// </summary>
/// <typeparam name="T">The type of the elements in the graph nodes.</typeparam>
public interface IGraph<T> 
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets or sets an optional action to be performed on each node during graph traversal.
    /// </summary>
    /// <remarks>
    /// This property allows assigning a delegate of type <c>OnVisit&lt;T&gt;</c> which
    /// can be invoked with the node's value as a parameter during operations like Depth-First Search (DFS).
    /// </remarks>
    Option<OnVisit<T>> OnVisitActionParameter { get; set; }

    /// <summary>
    /// Gets the total count of nodes in the graph.
    /// </summary>
    /// <remarks>
    /// This property provides the number of nodes currently present in the graph.
    /// It can be used to determine the size of the graph or validate operations based on node count.
    /// </remarks>
    int NodesCount { get; }

    /// <summary>
    /// Determines whether the graph contains a node with the specified value.
    /// </summary>
    /// <param name="value">The value of the node to search for in the graph.</param>
    /// <returns>true if the graph contains the node; otherwise, false.</returns>
    bool ContainsNode(T value);

    /// <summary>
    /// Determines whether there is a connection between two specified nodes in the graph.
    /// </summary>
    /// <param name="start">The starting node.</param>
    /// <param name="end">The target node.</param>
    /// <returns>true if the nodes are connected; otherwise, false.</returns>
    bool AreConnected(T start, T end);

    /// <summary>
    /// Performs a depth-first search (DFS) on a graph starting from the specified node.
    /// The method traverses the graph and optionally executes a specified action on each node encountered.
    /// </summary>
    /// <param name="start">The node from which the DFS will begin.</param>
    void TraverseDfs(T start);

    /// <summary>
    /// Performs a breadth-first search (BFS) on a graph starting from the specified node.
    /// The method traverses the graph and optionally executes a specified action on each node encountered.
    /// </summary>
    /// <param name="start">The node from which the BFS will begin.</param>
    void TraverseBfs(T start);

    /// <summary>
    /// Creates a modifiable version of the current graph.
    /// </summary>
    /// <returns>An <see cref="IGraphMod{T}"/> instance that represents a modifiable version of the current graph.</returns>
    IGraphMod<T> Mod();

    /// <summary>
    /// Transforms the current graph instance into a path search object.
    /// This allows for path search operations on the graph.
    /// </summary>
    /// <returns>An <see cref="IPathSearch{T}"/> instance that can be used to perform path search operations on the graph.</returns>
    IPathSearch<T> ToPathSearch();

    /// <summary>
    /// Transforms the current graph instance into a path search object using a specified path search strategy.
    /// This allows for execution of search algorithms within the graph using the provided strategy.
    /// </summary>
    /// <param name="shortestPathSearchStrategy">The strategy to be used for path searching within the graph.</param>
    /// <returns>An <see cref="IPathSearch{T}"/> instance configured with the specified strategy for searching paths in the graph.</returns>
    IPathSearch<T> ToPathSearch(IShortestPathSearchStrategy<T> shortestPathSearchStrategy);
}