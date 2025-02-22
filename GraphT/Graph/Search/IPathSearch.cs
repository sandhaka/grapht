using Monads.Optional;

namespace GraphT.Graph.Search;

/// <summary>
/// Interface defining methods and properties for performing path search operations
/// within a graph. It utilizes a search strategy to find the path from a start
/// node to a target node.
/// </summary>
/// <typeparam name="TK">The type of node keys within the graph, which must implement IEquatable.</typeparam>
public interface IPathSearch<TK>
    where TK : IEquatable<TK>
{
    /// <summary>
    /// Gets or sets the path search strategy.
    /// </summary>
    /// <remarks>
    /// The <c>ShortestPathSearchStrategy</c> property defines the strategy used for searching paths
    /// within a graph. This property must implement the <see cref="IShortestPathSearchStrategy{TK}"/>
    /// interface, which is designed to encapsulate different pathfinding algorithms or processes.
    /// The chosen strategy affects how the path is calculated or selected, impacting both the
    /// efficiency and the result of the search operation. Default implementation is Dijkstra algorithm.
    /// </remarks>
    IShortestPathSearchStrategy<TK> ShortestPathSearchStrategy { get; set; }

    /// <summary>
    /// Searches for a path from a starting node to a target node in a graph and returns the result of the search.
    /// </summary>
    /// <param name="start">The starting node of the path search.</param>
    /// <param name="target">The target node of the path search.</param>
    /// <param name="result">An optional result that will contain the search result if a path is found.</param>
    /// <returns>
    /// True if a path from the start to the target is found; otherwise, false.
    /// </returns>
    bool Search(TK start, TK target, out Option<SearchResult<TK>> result);
}