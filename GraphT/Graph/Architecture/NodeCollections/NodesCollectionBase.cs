using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections.Abstractions;

namespace GraphT.Graph.Architecture.NodeCollections;

internal abstract class NodesCollectionBase<T> : INodeCollection<T> 
    where T : IEquatable<T>
{
    protected readonly ISet<Node<T>> Nodes;
    private readonly Random _random = new();
    
    protected NodesCollectionBase(ISet<Node<T>> nodes)
    {
        Nodes = nodes;
    }
    
    public IReadOnlySet<T> Values => Nodes.Select(n => n.Value).ToHashSet();
    public int NodesCount => Nodes.Count;
    
    public T Value(int index) => Nodes.ElementAt(index).Value;
    
    public bool Contains(T value) => Nodes.Any(node => node.Value.Equals(value));
    
    public Node<T>[] ToArray() => Nodes.ToArray();

    public Node<T> this[T value]
    {
        get
        {
            using var enumerator = Nodes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value.Equals(value))
                    return enumerator.Current;
            }
            
            throw new InvalidOperationException();
        }
    }
    
    public Node<T> RandomNode() => Nodes.ElementAt(_random.Next(NodesCount));
}