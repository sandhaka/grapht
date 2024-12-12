using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;

namespace GraphT.Graph.Architecture.Implementations;

internal sealed class MutableGraph<T> : GraphBase<T>
    where T : IEquatable<T>
{
    private MutableGraph(IEnumerable<Node<T>> nodes) : base(new MutableNodeCollection<T>(nodes))
    {
    }
    
    public static MutableGraph<T> Create(IEnumerable<Node<T>> nodes)
    {
        var g = new MutableGraph<T>(nodes);
        
        return g;
    }
}