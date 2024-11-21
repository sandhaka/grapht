using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;

namespace GraphT.Graph.Architecture.Implementations;

internal sealed class ReadOnlyGraph<T> : GraphBase<T> 
    where T : IEquatable<T>
{
    private ReadOnlyGraph(IEnumerable<Node<T>> nodes)
    {
        NodesCollection = new FrozenNodeCollection<T>(nodes);
    }

    public static ReadOnlyGraph<T> Create(IEnumerable<Node<T>> nodes)
    {
        var g = new ReadOnlyGraph<T>(nodes);
        
        return g;
    }
}