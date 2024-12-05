using GraphT.Graph.Exceptions;
using GraphT.Graph.Parameters;
using Monads.Optional;

namespace GraphT.Graph.Search.Strategies;

public class AStar<T> : IPathSearchStrategy<T> 
    where T : IEquatable<T>
{
    public string Name => "AStar";

    public Option<Heuristic<T>> Heuristic { get; set; }
    
    public bool Run(IPathSearchContext<T> context, out Option<SearchResult<T>> result)
    {
        if (Heuristic.IsNone)
            throw new NoHeuristicDefinedException();
            
        throw new NotImplementedException();
    }
}