using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;
using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Algorithms;

internal class Prims<T>
    where T : IEquatable<T>
{
    private readonly FrozenNodeCollection<T> _nodesCollection;

    public Prims(FrozenNodeCollection<T> nodesCollection)
    {
        _nodesCollection = nodesCollection;
    }

    public Option<HashSet<Node<T>>> Mst()
    {
        var queue = new PriorityQueue<(T from, Edge<T> edge), decimal>();
        var visited = _nodesCollection.Values.ToDictionary(v => v, v => false);
        var mst = new HashSet<EdgeData<T>>();
        
        var node = _nodesCollection.RandomNode();
        EnqueueEdges(node, queue, visited);

        while (queue.TryDequeue(out var i, out _) && mst.Count != _nodesCollection.NodesCount - 1)
        {
            if (visited[i.edge.To.Value])
                continue;
            
            mst.Add(new EdgeData<T>(i.from, i.edge.Cost, i.edge.To.Value));
            EnqueueEdges(i.edge.To, queue, visited);
        }

        if (mst.Count != _nodesCollection.NodesCount - 1)
            return Option<HashSet<Node<T>>>.None();
        
        return BuildMstGraphNodes(mst);
    }

    private static void EnqueueEdges(Node<T> node, PriorityQueue<(T, Edge<T>), decimal> pq, Dictionary<T, bool> visited)
    {
        var span = node.Edges.Span;
        visited[node.Value] = true;
        
        foreach (var edge in span)
        {
            if (!visited[edge.To.Value])
                pq.Enqueue((node.Value, edge), edge.Cost);
        }
    }

    private static HashSet<Node<T>> BuildMstGraphNodes(HashSet<EdgeData<T>> mst)
    {
        // Ensure edges of the mst are two-way
        MakeUndirected(mst);
        
        // The mst data grouped by nodes
        var edgesGroups = mst.GroupBy(e => e.From).ToDictionary(g => g.Key, g => g.ToArray()); 
        // New nodes of the mst graph
        var nodes = edgesGroups.Select(n => new Node<T>(n.Key)).ToHashSet(); 

        foreach (var n in nodes)
        {
            var edgesData = edgesGroups[n.Value]; 
            // Allocate edges array of edgesData length
            var edges = new Edge<T>[edgesData.Length]; 
            var i = 0;

            for (var j = 0; j < edgesData.Length; j++)
            {
                var to = nodes.First(child => child.Value.Equals(edgesData[j].To));
                edges[i++] = new Edge<T>(n, to, edgesData[j].Cost);
            }
            
            n.Edges = new Memory<Edge<T>>(edges);
        }

        return nodes;
    }

    private static void MakeUndirected(HashSet<EdgeData<T>> edges)
    {
        for (var i = 0; i < edges.Count; i++)
        {
            var edge = edges.ElementAt(i);
            edges.Add(new EdgeData<T>(edge.To, edge.Cost, edge.From));
        }
    }
}