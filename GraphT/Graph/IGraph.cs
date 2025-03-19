using GraphT.Graph.Dto;
using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph;

/// <summary>
/// Represents a generic graph interface.
/// </summary>
/// <typeparam name="TK">The type of the elements in the graph nodes.</typeparam>
public interface IGraph<TK> 
    where TK : IEquatable<TK>
{
    /// <summary>
    /// Gets or sets an optional action to be performed on each node during graph traversal.
    /// </summary>
    /// <remarks>
    /// This property allows assigning a delegate of type <see cref="OnVisit{TK}"/> which
    /// can be invoked with the node's key as a parameter during operations like Depth-First Search (DFS).
    /// </remarks>
    Option<OnVisit<TK>> OnVisitActionParameter { get; set; }

    /// <summary>
    /// Gets the total count of nodes in the graph.
    /// </summary>
    /// <remarks>
    /// This property provides the number of nodes currently present in the graph.
    /// It can be used to determine the size of the graph or validate operations based on node count.
    /// </remarks>
    int NodesCount { get; }

    /// <summary>
    /// Gets the total number of edges in the graph.
    /// </summary>
    /// <remarks>
    /// This property represents the count of all edges present in the graph, which may vary
    /// based on the specific implementation and graph type (directed or undirected).
    /// For an undirected graph, each edge is counted once, whereas in a directed graph,
    /// the edge direction is taken into account.
    /// </remarks>
    int EdgesCount { get; }

    /// <summary>
    /// Determines whether the graph contains a node with the specified key.
    /// </summary>
    /// <param name="key">The key of the node to search for in the graph.</param>
    /// <returns>true if the graph contains the node; otherwise, false.</returns>
    bool ContainsNode(TK key);

    /// <summary>
    /// Determines whether there is a connection between two specified nodes in the graph.
    /// </summary>
    /// <param name="start">The starting node.</param>
    /// <param name="end">The target node.</param>
    /// <returns>true if the nodes are connected; otherwise, false.</returns>
    bool AreConnected(TK start, TK end);

    /// <summary>
    /// Performs a depth-first search (DFS) on a graph starting from the specified node.
    /// The method traverses the graph and optionally executes a specified action on each node encountered.
    /// </summary>
    /// <param name="start">The node from which the DFS will begin.</param>
    void TraverseDfs(TK start);

    /// <summary>
    /// Performs a breadth-first search (BFS) on a graph starting from the specified node.
    /// The method traverses the graph and optionally executes a specified action on each node encountered.
    /// </summary>
    /// <param name="start">The node from which the BFS will begin.</param>
    void TraverseBfs(TK start);

    /// <summary>
    /// Creates a modifiable version of the current graph.
    /// </summary>
    /// <returns>An <see cref="IGraphMod{TK}"/> instance that represents a modifiable version of the current graph.</returns>
    IGraphMod<TK> Mod();

    /// <summary>
    /// Transforms the current graph instance into a path search object.
    /// This allows for path search operations on the graph.
    /// </summary>
    /// <returns>An <see cref="IPathSearch{TK}"/> instance that can be used to perform path search operations on the graph.</returns>
    IPathSearch<TK> ToPathSearch();

    /// <summary>
    /// Transforms the current graph instance into a path search object using a specified path search strategy.
    /// This allows for execution of search algorithms within the graph using the provided strategy.
    /// </summary>
    /// <param name="shortestPathSearchStrategy">The strategy to be used for path searching within the graph.</param>
    /// <returns>An <see cref="IPathSearch{TK}"/> instance configured with the specified strategy for searching paths in the graph.</returns>
    IPathSearch<TK> ToPathSearch(IShortestPathSearchStrategy<TK> shortestPathSearchStrategy);

    /// <summary>
    /// Reduces the current graph to its Minimum Spanning Tree (MST).
    /// </summary>
    /// <returns>
    /// A new graph that represents the Minimum Spanning Tree of the current graph.
    /// If a MST not exists, returns the current graph.
    /// </returns>
    IGraph<TK> ReduceToMst();

    /// <summary>
    /// Determines whether the graph contains any cycles.
    /// </summary>
    /// <returns>true if the graph contains a cycle; otherwise, false.</returns>
    bool IsCyclic();
    
    /// <summary>
    /// Determines whether the graph is undirected
    /// </summary>
    /// <returns>true if the graph is undirected; otherwise, false.</returns>
    bool IsUndirected();

    /// <summary>
    /// Retrieves a read-only collection of all edges in the graph.
    /// </summary>
    /// <returns>A list of edges, where each edge contains the source, destination, and cost.</returns>
    IReadOnlyList<EdgeData<TK>> AllEdges();
}