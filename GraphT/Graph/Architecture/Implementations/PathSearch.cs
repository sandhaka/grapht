using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal class PathSearch<T> : IPathSearch<T>
    where T : IEquatable<T>
{
    private readonly IGraphComponents<T> _graph;

    // Using Dijkstra as default search strategy
    public IShortestPathSearchStrategy<T> ShortestPathSearchStrategy { get; set; }

    public PathSearch(IGraphComponents<T> graph)
    {
        _graph = graph;
        ShortestPathSearchStrategy = new Dijkstra<T>(); 
    }
    
    public PathSearch(IGraphComponents<T> graph, IShortestPathSearchStrategy<T> shortestPathSearchStrategy)
    {
        _graph = graph;
        ShortestPathSearchStrategy = shortestPathSearchStrategy;
    }

    public bool Search(T start, T target, out Option<SearchResult<T>> result)
    {
        var context = new PathSearchContext<T>(_graph, start, target);
        return ShortestPathSearchStrategy.Run(context, out result);
    }
}