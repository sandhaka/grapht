using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal class Dijkstra<T> : IPathSearchStrategy<T>
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

            context.ForeachNeighbors(nv, (nextValue, cost) =>
            {
                if (visited[nextValue])
                    return;
                
                var distance = distances[nv] + cost;

                if (distance >= distances[nextValue]) 
                    return;
                
                pathMarker[nextValue] = nv;
                distances[nextValue] = distance;
                queue.Enqueue(nextValue, distance);
            });

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