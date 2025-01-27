using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;
using GraphT.Graph.Dto;

namespace GraphT.Graph.Architecture.Implementations;

internal class Prims<T>
    where T : IEquatable<T>
{
    private readonly FrozenNodeCollection<T> _nodesCollection;

    public Prims(FrozenNodeCollection<T> nodexCollection)
    {
        _nodesCollection = nodexCollection;
    }

    public HashSet<EdgeData<T>> FindMst()
    {
        var queue = new PriorityQueue<(T from, Edge<T> edge), decimal>();
        var visited = _nodesCollection.Values.ToDictionary(v => v, v => false);
        var mst = new HashSet<EdgeData<T>>();
        
        var node = _nodesCollection.RandomNode();
        EnqueueEdges(node, queue, visited);

        while (queue.TryDequeue(out var i, out _))
        {
            if (visited[i.edge.To.Value])
                continue;
            
            mst.Add(new EdgeData<T>(node.Value, i.edge.Cost, i.edge.To.Value));
            EnqueueEdges(node, queue, visited);
        }

        if (mst.Count != _nodesCollection.NodesCount - 1)
            return mst;

        return [];
    }

    private static void EnqueueEdges(Node<T> node, PriorityQueue<(T, Edge<T>), decimal> pq, Dictionary<T, bool> visited)
    {
        var span = node.Edges.Span;
        visited[node.Value] = true;
        
        foreach (var edge in span)
        {
            if (visited[edge.To.Value])
                continue;
            
            pq.Enqueue((node.Value, edge), edge.Cost);
        }
    }
}