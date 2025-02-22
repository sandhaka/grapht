using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Architecture.Search;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal class PathSearch<TK> : IPathSearch<TK>
    where TK : IEquatable<TK>
{
    private readonly IGraphComponents<TK> _graph;

    // Using Dijkstra as default search strategy
    public IShortestPathSearchStrategy<TK> ShortestPathSearchStrategy { get; set; }

    public PathSearch(IGraphComponents<TK> graph)
    {
        _graph = graph;
        ShortestPathSearchStrategy = new Dijkstra<TK>(); 
    }
    
    public PathSearch(IGraphComponents<TK> graph, IShortestPathSearchStrategy<TK> shortestPathSearchStrategy)
    {
        _graph = graph;
        ShortestPathSearchStrategy = shortestPathSearchStrategy;
    }

    public bool Search(TK start, TK target, out Option<SearchResult<TK>> result)
    {
        var context = new PathSearchContext<TK>(_graph, start, target);
        return ShortestPathSearchStrategy.Run(context, out result);
    }
}