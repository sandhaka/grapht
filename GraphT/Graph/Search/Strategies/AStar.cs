using GraphT.Graph.Exceptions;
using GraphT.Graph.Parameters;
using GraphT.Graph.Search.Context;
using Monads.Optional;

namespace GraphT.Graph.Search.Strategies;

public class AStar<TK> : IShortestPathSearchStrategy<TK> 
    where TK : IEquatable<TK>
{
    public string Name => "AStar";

    public Option<Heuristic<TK>> Heuristic { get; set; }

    public AStar(Heuristic<TK> heuristic)
    {
        Heuristic = heuristic;
    }
    
    public bool Run(IPathSearchContext<TK> context, out Option<SearchResult<TK>> result)
    {
        var heuristicFunc = Heuristic.Reduce(() => throw new NoHeuristicDefinedException());
        var q = new PriorityQueue<TK, decimal>([(context.Start, 0)]);
        var exploredSet = new Dictionary<TK, decimal> { [context.Start] = 0 };
        var pathMarker = context.NodeKeys.ToDictionary(x => x, _ => default(TK));
        
        result = Option<SearchResult<TK>>.None();

        while (q.Count > 0)
        {
            var n = q.Dequeue();

            if (n.Equals(context.Target))
            {
                result = new SearchResult<TK>
                {
                    Path = GenPath(pathMarker, context.Target).Reverse().ToList(),
                    TotalCost = exploredSet[context.Target]
                };

                return true;
            }

            foreach (var edge in context.NodeEdges(n))
            {
                var nodeKey = edge.NodeKey;
                var cost = edge.Cost;
                exploredSet.TryAdd(nodeKey, decimal.MaxValue);
                var nc = exploredSet[n] + cost;
                if (nc >= exploredSet[nodeKey]) continue;
                exploredSet[nodeKey] = nc;
                var estimatedCost = nc + heuristicFunc(nodeKey, context);
                q.Enqueue(nodeKey, estimatedCost);
                pathMarker[nodeKey] = n;
            }
        }

        return false;
    }

    private static IEnumerable<TK> GenPath(Dictionary<TK,TK?> pathMarker, TK target)
    {
        TK? n = target;
        yield return n;

        while (n is not null && pathMarker[n] is not null)
        {
            n = pathMarker[n];
            if (n is not null)
                yield return n;
        }
    }
}