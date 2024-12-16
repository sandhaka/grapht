using GraphT.Graph.Exceptions;
using GraphT.Graph.Parameters;
using GraphT.Graph.Search.Context;
using Monads.Optional;

namespace GraphT.Graph.Search.Strategies;

public class AStar<T> : IShortestPathSearchStrategy<T> 
    where T : IEquatable<T>
{
    public string Name => "AStar";

    public Option<Heuristic<T>> Heuristic { get; set; }

    public AStar(Heuristic<T> heuristic)
    {
        Heuristic = heuristic;
    }
    
    public bool Run(IPathSearchContext<T> context, out Option<SearchResult<T>> result)
    {
        var heuristicFunc = Heuristic.Reduce(() => throw new NoHeuristicDefinedException());
        var q = new PriorityQueue<T, decimal>([(context.Start, 0)]);
        var exploredSet = new Dictionary<T, decimal> { [context.Start] = 0 };
        var pathMarker = context.NodeValues.ToDictionary(x => x, _ => default(T));
        
        result = Option<SearchResult<T>>.None();

        while (q.Count > 0)
        {
            var n = q.Dequeue();

            if (n.Equals(context.Target))
            {
                result = new SearchResult<T>
                {
                    Path = GenPath(pathMarker, context.Target).Reverse().ToList(),
                    TotalCost = exploredSet[context.Target]
                };

                return true;
            }

            foreach (var edge in context.NodeEdges(n))
            {
                var nodeValue = edge.NodeValue;
                var cost = edge.Cost;
                exploredSet.TryAdd(nodeValue, decimal.MaxValue);
                var nc = exploredSet[n] + cost;
                if (nc >= exploredSet[nodeValue]) continue;
                exploredSet[nodeValue] = nc;
                var estimatedCost = nc + heuristicFunc(nodeValue, context);
                q.Enqueue(nodeValue, estimatedCost);
                pathMarker[nodeValue] = n;
            }
        }

        return false;
    }

    private static IEnumerable<T> GenPath(Dictionary<T,T?> pathMarker, T target)
    {
        T? n = target;
        yield return n;

        while (n is not null && pathMarker[n] is not null)
        {
            n = pathMarker[n];
            if (n is not null)
                yield return n;
        }
    }
}