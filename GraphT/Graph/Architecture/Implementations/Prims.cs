using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;
using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal class Prims<TK>
    where TK : IEquatable<TK>
{
    private readonly FrozenNodeCollection<TK> _nodesCollection;

    public Prims(FrozenNodeCollection<TK> nodesCollection)
    {
        _nodesCollection = nodesCollection;
    }

    public Option<HashSet<Node<TK>>> Mst()
    {
        var queue = new PriorityQueue<(TK from, Edge<TK> edge), decimal>();
        var visited = _nodesCollection.Keys.ToDictionary(v => v, v => false);
        var mst = new HashSet<EdgeData<TK>>();
        
        var node = _nodesCollection.RandomNode();
        EnqueueEdges(node, queue, visited);

        while (queue.TryDequeue(out var i, out _) && mst.Count != _nodesCollection.NodesCount - 1)
        {
            if (visited[i.edge.To.Key])
                continue;
            
            mst.Add(new EdgeData<TK>(i.from, i.edge.Cost, i.edge.To.Key));
            EnqueueEdges(i.edge.To, queue, visited);
        }

        if (mst.Count != _nodesCollection.NodesCount - 1)
            return Option<HashSet<Node<TK>>>.None();
        
        return BuildMstGraphNodes(mst);
    }

    private static void EnqueueEdges(Node<TK> node, PriorityQueue<(TK, Edge<TK>), decimal> pq, Dictionary<TK, bool> visited)
    {
        var span = node.Edges.Span;
        visited[node.Key] = true;
        
        foreach (var edge in span)
        {
            if (!visited[edge.To.Key])
                pq.Enqueue((node.Key, edge), edge.Cost);
        }
    }

    private static HashSet<Node<TK>> BuildMstGraphNodes(HashSet<EdgeData<TK>> mst)
    {
        // Ensure edges of the mst are two-way
        MakeUndirected(mst);
        
        // The mst data grouped by nodes
        var edgesGroups = mst.GroupBy(e => e.From).ToDictionary(g => g.Key, g => g.ToArray()); 
        // New nodes of the mst graph
        var nodes = edgesGroups.Select(n => new Node<TK>(n.Key)).ToHashSet(); 

        foreach (var n in nodes)
        {
            var edgesData = edgesGroups[n.Key]; 
            // Allocate edges array of edgesData length
            var edges = new Edge<TK>[edgesData.Length]; 
            var i = 0;

            for (var j = 0; j < edgesData.Length; j++)
            {
                var to = nodes.First(child => child.Key.Equals(edgesData[j].To));
                edges[i++] = new Edge<TK>(n, to, edgesData[j].Cost);
            }
            
            n.Edges = new Memory<Edge<TK>>(edges);
        }

        return nodes;
    }

    private static void MakeUndirected(HashSet<EdgeData<TK>> edges)
    {
        for (var i = 0; i < edges.Count; i++)
        {
            var edge = edges.ElementAt(i);
            edges.Add(new EdgeData<TK>(edge.To, edge.Cost, edge.From));
        }
    }
}