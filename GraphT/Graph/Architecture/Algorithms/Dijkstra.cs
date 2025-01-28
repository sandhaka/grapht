using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using GraphT.Graph.Search.Context;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Algorithms;

internal class Dijkstra<T> : IShortestPathSearchStrategy<T>
    where T : IEquatable<T>
{
    public string Name => "Dijkstra";

    // Dijkstra has no heuristic
    public Option<Heuristic<T>> Heuristic { get; set; } = Option<Heuristic<T>>.None();

    public bool Run(IPathSearchContext<T> context, out Option<SearchResult<T>> result)
    {
        var found = false;
        var queue = new PriorityQueue<T, decimal>();

        var (visited, distances, pathMarker) = DataStructPrepare(context.NodeValues);
        
        queue.Enqueue(context.Start, 0);
        distances[context.Start] = 0;

        while (queue.Count > 0)
        {
            var nv = queue.Dequeue();
            visited[nv] = true;

            foreach (var edge in context.NodeEdges(nv))
            {
                var nodeValue = edge.NodeValue;
                var cost = edge.Cost;
                
                if (visited[nodeValue])
                    continue;
                
                var distance = distances[nv] + cost;

                if (distance >= distances[nodeValue]) 
                    continue;
                
                pathMarker[nodeValue] = nv;
                distances[nodeValue] = distance;
                queue.Enqueue(nodeValue, distance);
            }

            if (!nv.Equals(context.Target)) 
                continue;
            
            found = true;
            break;
        }
        
        result = Option<SearchResult<T>>.None();
        
        if (found is false)
            return false;
        
        result = new SearchResult<T>
        {
            Path = GenPath(pathMarker, context.Target).Reverse().ToList(),
            TotalCost = distances[context.Target]
        };

        return true;
    }

    private static (Dictionary<T, bool> visited, Dictionary<T, decimal> distances, Dictionary<T, T?> pathMarker)
        DataStructPrepare(IReadOnlySet<T> nodeValuesSet)
    {
        return (
            visited: nodeValuesSet.ToDictionary(x => x, _ => false),
            distances: nodeValuesSet.ToDictionary(x => x, _ => decimal.MaxValue),
            pathMarker: nodeValuesSet.ToDictionary(x => x, _ => default(T))
        );
    }

    private static IEnumerable<T> GenPath(Dictionary<T, T?> prev, T target)
    {
        T? n = target;
        yield return n;

        while (n is not null && prev[n] is not null)
        {
            n = prev[n];
            if (n is not null)
                yield return n;
        }
    }
}