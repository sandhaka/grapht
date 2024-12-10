using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections.Abstractions;

namespace GraphT.Graph.Architecture.NodeCollections;

internal abstract class NodesCollectionBase<T> : INodeCollection<T> 
    where T : IEquatable<T>
{
    private readonly ISet<Node<T>> _nodes;
    
    protected NodesCollectionBase(ISet<Node<T>> nodes)
    {
        _nodes = nodes;
    }
    
    public IReadOnlySet<T> Values => _nodes.Select(n => n.Value).ToHashSet();
    public int NodesCount => _nodes.Count;
    public T Value(int index) => _nodes.ElementAt(index).Value;
    public bool Contains(T value) => _nodes.Any(node => node.Value.Equals(value));
    public Node<T> this[T value]
    {
        get
        {
            using var enumerator = _nodes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.Equals(value))
                    return enumerator.Current;
            }
            
            throw new InvalidOperationException();
        }
    }
}