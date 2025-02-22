using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using GraphT.Graph.Search.Context;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Search;

internal class Dijkstra<TK> : IShortestPathSearchStrategy<TK>
    where TK : IEquatable<TK>
{
    public string Name => "Dijkstra";

    // Dijkstra has no heuristic
    public Option<Heuristic<TK>> Heuristic { get; set; } = Option<Heuristic<TK>>.None();

    public bool Run(IPathSearchContext<TK> context, out Option<SearchResult<TK>> result)
    {
        var found = false;
        var queue = new PriorityQueue<TK, decimal>();

        var (visited, distances, pathMarker) = DataStructPrepare(context.NodeKeys);
        
        queue.Enqueue(context.Start, 0);
        distances[context.Start] = 0;

        while (queue.Count > 0)
        {
            var nv = queue.Dequeue();
            visited[nv] = true;

            foreach (var edge in context.NodeEdges(nv))
            {
                var nodeKey = edge.NodeKey;
                var cost = edge.Cost;
                
                if (visited[nodeKey])
                    continue;
                
                var distance = distances[nv] + cost;

                if (distance >= distances[nodeKey]) 
                    continue;
                
                pathMarker[nodeKey] = nv;
                distances[nodeKey] = distance;
                queue.Enqueue(nodeKey, distance);
            }

            if (!nv.Equals(context.Target)) 
                continue;
            
            found = true;
            break;
        }
        
        result = Option<SearchResult<TK>>.None();
        
        if (found is false)
            return false;
        
        result = new SearchResult<TK>
        {
            Path = GenPath(pathMarker, context.Target).Reverse().ToList(),
            TotalCost = distances[context.Target]
        };

        return true;
    }

    private static (Dictionary<TK, bool> visited, Dictionary<TK, decimal> distances, Dictionary<TK, TK?> pathMarker) DataStructPrepare(IReadOnlySet<TK> nodeKeysSet)
    {
        return (
            visited: nodeKeysSet.ToDictionary(x => x, _ => false),
            distances: nodeKeysSet.ToDictionary(x => x, _ => decimal.MaxValue),
            pathMarker: nodeKeysSet.ToDictionary(x => x, _ => default(TK))
        );
    }

    private static IEnumerable<TK> GenPath(Dictionary<TK, TK?> prev, TK target)
    {
        TK? n = target;
        yield return n;

        while (n is not null && prev[n] is not null)
        {
            n = prev[n];
            if (n is not null)
                yield return n;
        }
    }
}