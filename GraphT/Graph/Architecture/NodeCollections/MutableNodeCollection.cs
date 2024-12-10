using System.Diagnostics;
using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections.Abstractions;

namespace GraphT.Graph.Architecture.NodeCollections;

[DebuggerDisplay("{NodesCount} nodes")]
internal sealed class MutableNodeCollection<T> : NodesCollectionBase<T>, IMutableNodeCollection<T> 
    where T : IEquatable<T>
{
    public MutableNodeCollection(IEnumerable<Node<T>> nodes) : base(nodes.ToHashSet())
    {
    }

    public void Add(T value)
    {
        throw new NotImplementedException();
    }

    public void Remove(T value)
    {
        throw new NotImplementedException();
    }
}