using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections.Abstractions;

namespace GraphT.Graph.Architecture.NodeCollections;

[DebuggerDisplay("{NodesCount} nodes")]
internal sealed class MutableNodeCollection<T> : IMutableNodeCollection<T> where T : IEquatable<T>
{
    private readonly HashSet<Node<T>> _nodes;
    
    [SetsRequiredMembers]
    public MutableNodeCollection(IEnumerable<Node<T>> nodes)
    {
        _nodes = nodes.ToHashSet();
    }

    public void Add(T value)
    {
        throw new NotImplementedException();
    }

    public void Remove(T value)
    {
        throw new NotImplementedException();
    }

    public int NodesCount => _nodes.Count;
    public bool Contains(T value) => _nodes.Any(node => node.Value.Equals(value));
    public Node<T> this[T value] => _nodes.Single(node => node.Value.Equals(value));
    public IReadOnlySet<T> Values => _nodes.Select(n => n.Value).ToHashSet();
}