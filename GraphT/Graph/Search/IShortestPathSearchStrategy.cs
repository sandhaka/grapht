using GraphT.Graph.Parameters;
using GraphT.Graph.Search.Context;
using Monads.Optional;

namespace GraphT.Graph.Search;

/// <summary>
/// Strategy interface for searching paths within a graph.
/// </summary>
/// <typeparam name="TK">The type of node keys within the graph.</typeparam>
public interface IShortestPathSearchStrategy<TK> 
    where TK : IEquatable<TK>
{
    /// <summary>
    /// Gets the name of the graph search strategy.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets or sets the heuristic function used to estimate the cost of reaching the target node.
    /// </summary>
    public Option<Heuristic<TK>> Heuristic { get; set; }
    
    /// <summary>
    /// Executes the graph search algorithm based on the implemented strategy.
    /// </summary>
    /// <typeparam name="TK">The type of node keys within the graph.</typeparam>
    /// <param name="context">The context that provides necessary data for the graph search operation.</param>
    /// <param name="result">SearchResult containing the path and the total cost if the search is successful.</param>
    /// <returns>True if the search finds a path to the target node; otherwise, false.</returns>
    bool Run(IPathSearchContext<TK> context, out Option<SearchResult<TK>> result);

}