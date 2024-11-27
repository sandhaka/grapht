using Monads.Optional;

namespace GraphT.Graph.Search;

public interface IPathSearch<T> 
    where T : IEquatable<T>
{
    IPathSearchStrategy<T> PathSearchStrategy { get; set; }
    bool Search(T start, T target, out Option<SearchResult<T>> result);
}