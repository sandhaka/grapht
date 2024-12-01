using Monads.Optional;

namespace GraphT.Graph.Search;

/// <summary>
/// Interface defining methods and properties for performing path search operations
/// within a graph. It utilizes a search strategy to find the path from a start
/// node to a target node.
/// </summary>
/// <typeparam name="T">The type of node values within the graph, which must implement IEquatable.</typeparam>
public interface IPathSearch<T>
    where T : IEquatable<T>
{
    /// <summary>
    /// Gets or sets the path search strategy.
    /// </summary>
    /// <remarks>
    /// The <c>PathSearchStrategy</c> property defines the strategy used for searching paths
    /// within a graph. This property must implement the <see cref="IPathSearchStrategy{T}"/>
    /// interface, which is designed to encapsulate different pathfinding algorithms or processes.
    /// The chosen strategy affects how the path is calculated or selected, impacting both the
    /// efficiency and the result of the search operation.
    /// </remarks>
    IPathSearchStrategy<T> PathSearchStrategy { get; set; }

    /// <summary>
    /// Searches for a path from a starting node to a target node in a graph and returns the result of the search.
    /// </summary>
    /// <param name="start">The starting node of the path search.</param>
    /// <param name="target">The target node of the path search.</param>
    /// <param name="result">An optional result that will contain the search result if a path is found.</param>
    /// <returns>
    /// True if a path from the start to the target is found; otherwise, false.
    /// </returns>
    bool Search(T start, T target, out Option<SearchResult<T>> result);
}