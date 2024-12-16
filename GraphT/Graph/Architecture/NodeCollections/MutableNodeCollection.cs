using System.Diagnostics;
using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections.Abstractions;
using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph.Architecture.NodeCollections;

[DebuggerDisplay("{NodesCount} nodes")]
internal sealed class MutableNodeCollection<T> : NodesCollectionBase<T>, IMutableNodeCollection<T> 
    where T : IEquatable<T>
{
    public MutableNodeCollection(IEnumerable<Node<T>> nodes) : base(nodes.ToHashSet())
    {
    }

    public void Add(T value, Option<EdgeTuple<T>[]> inEdges, Option<EdgeTuple<T>[]> outEdges)
    {
        if (inEdges.IsNone && outEdges.IsNone)
            throw new ArgumentException("Trying to add orphan node! Either inEdges or outEdges must be provided");
        
        if (this.Contains(value))
            throw new ArgumentException("Node with this value already exists");
        
        AddNode(value, inEdges.Reduce([]), outEdges.Reduce([]));
    }

    public void Remove(T value)
    {
        if (!this.Contains(value))
            return; //  silent approach. Or throw an exception?
        Nodes.Remove(Nodes.First(n => n.Value.Equals(value)));
        foreach (var node in Nodes)
        {
            if(node.Edges.IsEmpty) continue;
            var index = FindEdgeIndex(node, value);
            if (index.IsNone) continue;
            var i = index.Reduce(() => throw new InvalidOperationException());
            
            var edges = node.Edges.Span.ToArray();
            Array.Copy(edges, i + 1, edges, i, edges.Length - i - 1);
            Array.Resize(ref edges, edges.Length - 1);
            node.Edges = new Memory<Edge<T>>(edges);
        }
    }

    private ValueOption<int> FindEdgeIndex(Node<T> node, T value)
    {
        var edges = node.Edges.Span;
        foreach (var edge in edges)
        {
            if (edge.To.Value.Equals(value))
                return edges.IndexOf(edge);
        }

        return ValueOption<int>.None();
    }

    private void AddNode(T value, EdgeTuple<T>[] inEdgesValues, EdgeTuple<T>[] outEdgesValues)
    {
        ValidateEdges(inEdgesValues);
        ValidateEdges(outEdgesValues);
        
        var node = new Node<T>(value);
        var outEdges = outEdgesValues.Select(edgeTuple => CreateEdge(edgeTuple.NodeValue, edgeTuple.Cost));
        node.Edges = new Memory<Edge<T>>(outEdges.ToArray());
        
        Nodes.Add(node);
        
        AddEdgesInConnectedNodes(inEdgesValues, node);
    }

    private void ValidateEdges(EdgeTuple<T>[] edgesValues)
    {
        if (edgesValues.Select(e => e.NodeValue).Any(nodeValue => !Contains(nodeValue)))
        {
            throw new ArgumentException("Edge to a node with a non existent value");
        }
    }
    
    private Edge<T> CreateEdge(T value, decimal cost)
    {
        var destinationNode = Nodes.First(n => n.Value.Equals(value));
        return new Edge<T>(destinationNode, cost);
    }

    private void AddEdgesInConnectedNodes(EdgeTuple<T>[] inEdges, Node<T> dest)
    {
        foreach (var (nodeValue, cost) in inEdges)
        {
            var node = Nodes.First(n => n.Value.Equals(nodeValue));

            if (node.Edges.IsEmpty)
            {
                node.Edges = new Memory<Edge<T>>([new Edge<T>(dest, cost)]);
                continue;
            }
            
            var updatedEdges = new Edge<T>[node.Edges.Length + 1];
            node.Edges.Span.CopyTo(updatedEdges);
            updatedEdges[^1] = new Edge<T>(dest, cost);
            node.Edges = new Memory<Edge<T>>(updatedEdges);
        }
    }
}