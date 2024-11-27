using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal class PathSearch<T>(IGraphComponents<T> graph) : IPathSearch<T>
    where T : IEquatable<T>
{
    // Using Dijkstra as default search strategy
    public IPathSearchStrategy<T> PathSearchStrategy { get; set; } = new Dijkstra<T>();

    public bool Search(T start, T target, out Option<SearchResult<T>> result)
    {
        var context = new PathSearchContext<T>(graph, start, target);
        return PathSearchStrategy.Run(context, out result);
    }
}