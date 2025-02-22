using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;
using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal sealed class MutableGraph<TK> : IGraphMod<TK>
    where TK : IEquatable<TK>
{
    private readonly IGraph<TK> _originalGraph; // Keep original reference can be useful for future usage
    private readonly MutableNodeCollection<TK> _nodes;
    
    private MutableGraph(IGraph<TK> originalGraph, IEnumerable<Node<TK>> nodes)
    {
        _originalGraph = originalGraph;
        _nodes = new MutableNodeCollection<TK>(nodes);
    }
    
    public static MutableGraph<TK> Create(IGraph<TK> originalGraph, IEnumerable<Node<TK>> nodes)
    {
        var g = new MutableGraph<TK>(originalGraph, nodes);
        
        return g;
    }

    public IGraph<TK> EndMod()
    {
        var readonlyGraph = ReadOnlyGraph<TK>.Create(_nodes.ToArray());
        
        readonlyGraph.OnVisitActionParameter = _originalGraph.OnVisitActionParameter;

        return readonlyGraph;
    }
    
    public IGraphMod<TK> AddNode(TK key, Option<EdgeTuple<TK>[]> inEdges, Option<EdgeTuple<TK>[]> outEdges)
    {
        _nodes.Add(key, inEdges, outEdges);
        return this;
    }

    public IGraphMod<TK> RemoveNode(TK key)
    {
        _nodes.Remove(key);
        return this;
    }
}