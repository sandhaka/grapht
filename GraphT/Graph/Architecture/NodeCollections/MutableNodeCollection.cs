using System.Diagnostics;
using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph.Architecture.NodeCollections;

[DebuggerDisplay("{NodesCount} nodes")]
internal sealed class MutableNodeCollection<TK> : NodesCollectionBase<TK>
    where TK : IEquatable<TK>
{
    public MutableNodeCollection(IEnumerable<Node<TK>> nodes) : base(nodes.ToHashSet())
    {
    }

    public void Add(TK key, Option<EdgeTuple<TK>[]> inEdges, Option<EdgeTuple<TK>[]> outEdges)
    {
        if (inEdges.IsNone && outEdges.IsNone)
            throw new ArgumentException("Trying to add orphan node! Either inEdges or outEdges must be provided");
        
        if (this.Contains(key))
            throw new ArgumentException("Node with this key already exists");
        
        AddNode(key, inEdges.Reduce([]), outEdges.Reduce([]));
    }

    public void Remove(TK key)
    {
        if (!this.Contains(key))
            return; //  silent approach. Or throw an exception?
        Nodes.Remove(Nodes.First(n => n.Key.Equals(key)));
        foreach (var node in Nodes)
        {
            if(node.Edges.IsEmpty) continue;
            var index = FindEdgeIndex(node, key);
            if (index.IsNone) continue;
            var i = index.Reduce(() => throw new InvalidOperationException());
            
            var edges = node.Edges.Span.ToArray();
            Array.Copy(edges, i + 1, edges, i, edges.Length - i - 1);
            Array.Resize(ref edges, edges.Length - 1);
            node.Edges = new Memory<Edge<TK>>(edges);
        }
    }

    private ValueOption<int> FindEdgeIndex(Node<TK> node, TK key)
    {
        var edges = node.Edges.Span;
        foreach (var edge in edges)
        {
            if (edge.To.Key.Equals(key))
                return edges.IndexOf(edge);
        }

        return ValueOption<int>.None();
    }

    private void AddNode(TK key, EdgeTuple<TK>[] inEdgesKeys, EdgeTuple<TK>[] outEdgesKeys)
    {
        ValidateEdges(inEdgesKeys);
        ValidateEdges(outEdgesKeys);
        
        var node = new Node<TK>(key);
        Nodes.Add(node);

        var outEdges = outEdgesKeys.Select(edgeTuple => CreateEdge(key, edgeTuple.NodeKey, edgeTuple.Cost));
        node.Edges = new Memory<Edge<TK>>(outEdges.ToArray());
        
        AddEdgesInConnectedNodes(inEdgesKeys, node);
    }

    private void ValidateEdges(EdgeTuple<TK>[] edgesKeys)
    {
        if (edgesKeys.Select(e => e.NodeKey).Any(nodeKey => !Contains(nodeKey)))
        {
            throw new ArgumentException("Edge to a node with a non existent key");
        }
    }
    
    private Edge<TK> CreateEdge(TK src, TK dest, decimal cost)
    {
        var sourceNode = Nodes.First(n => n.Key.Equals(src));
        var destinationNode = Nodes.First(n => n.Key.Equals(dest));
        return new Edge<TK>(sourceNode, destinationNode, cost);
    }

    private void AddEdgesInConnectedNodes(EdgeTuple<TK>[] inEdges, Node<TK> dest)
    {
        foreach (var (nodeKey, cost) in inEdges)
        {
            var from = Nodes.First(n => n.Key.Equals(nodeKey));

            if (from.Edges.IsEmpty)
            {
                from.Edges = new Memory<Edge<TK>>([new Edge<TK>(from, dest, cost)]);
                continue;
            }
            
            var updatedEdges = new Edge<TK>[from.Edges.Length + 1];
            from.Edges.Span.CopyTo(updatedEdges);
            updatedEdges[^1] = new Edge<TK>(from, dest, cost);
            from.Edges = new Memory<Edge<TK>>(updatedEdges);
        }
    }
}