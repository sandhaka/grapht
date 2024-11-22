using Monads.Optional;

namespace GraphT.Graph.Search;

public interface IPathSearch<T> 
    where T : IEquatable<T>
{
    IGraphSearchStrategy<T> GraphSearchStrategy { get; set; }
    bool Search(T start, T target, out Option<SearchResult<T>> result);
}