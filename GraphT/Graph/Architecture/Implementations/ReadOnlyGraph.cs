using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;

namespace GraphT.Graph.Architecture.Implementations;

internal sealed class ReadOnlyGraph<TK> : GraphBase<TK> 
    where TK : IEquatable<TK>
{
    private ReadOnlyGraph(IEnumerable<Node<TK>> nodes) : base(new FrozenNodeCollection<TK>(nodes))
    {
    }

    public static ReadOnlyGraph<TK> Create(IEnumerable<Node<TK>> nodes)
    {
        var g = new ReadOnlyGraph<TK>(nodes);
        
        return g;
    }
}