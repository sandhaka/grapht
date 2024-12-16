using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;
using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal sealed class MutableGraph<T> : IGraphMod<T>
    where T : IEquatable<T>
{
    private readonly IGraph<T> _originalGraph; // Keep original reference can be useful for future usage
    private readonly MutableNodeCollection<T> _nodes;
    
    private MutableGraph(IGraph<T> originalGraph, IEnumerable<Node<T>> nodes)
    {
        _originalGraph = originalGraph;
        _nodes = new MutableNodeCollection<T>(nodes);
    }
    
    public static MutableGraph<T> Create(IGraph<T> originalGraph, IEnumerable<Node<T>> nodes)
    {
        var g = new MutableGraph<T>(originalGraph, nodes);
        
        return g;
    }

    public IGraph<T> EndMod()
    {
        var readonlyGraph = ReadOnlyGraph<T>.Create(_nodes.ToArray());
        
        readonlyGraph.OnVisitActionParameter = _originalGraph.OnVisitActionParameter;

        return readonlyGraph;
    }
    
    public IGraphMod<T> AddNode(T value, Option<EdgeTuple<T>[]> inEdges, Option<EdgeTuple<T>[]> outEdges)
    {
        _nodes.Add(value, inEdges, outEdges);
        return this;
    }

    public IGraphMod<T> RemoveNode(T value)
    {
        _nodes.Remove(value);
        return this;
    }
}