using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal class PathSearch<T> : IPathSearch<T>
    where T : IEquatable<T>
{
    private readonly IGraphComponents<T> _graph;

    // Using Dijkstra as default search strategy
    public IPathSearchStrategy<T> PathSearchStrategy { get; set; }

    public PathSearch(IGraphComponents<T> graph)
    {
        _graph = graph;
        PathSearchStrategy = new Dijkstra<T>(); 
    }
    
    public PathSearch(IGraphComponents<T> graph, IPathSearchStrategy<T> pathSearchStrategy)
    {
        _graph = graph;
        PathSearchStrategy = pathSearchStrategy;
    }

    public bool Search(T start, T target, out Option<SearchResult<T>> result)
    {
        var context = new PathSearchContext<T>(_graph, start, target);
        return PathSearchStrategy.Run(context, out result);
    }
}