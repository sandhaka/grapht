using System.Collections;
using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections.Abstractions;

namespace GraphT.Graph.Architecture.NodeCollections;

internal abstract class NodesCollectionBase<TK> : INodeCollection<TK>, IEnumerable<Node<TK>>
    where TK : IEquatable<TK>
{
    protected readonly ISet<Node<TK>> Nodes;
    private readonly Random _random = new();
    
    protected NodesCollectionBase(ISet<Node<TK>> nodes)
    {
        Nodes = nodes;
    }
    
    public IReadOnlySet<TK> Keys => Nodes.Select(n => n.Key).ToHashSet();
    public int NodesCount => Nodes.Count;
    
    public TK Key(int index) => Nodes.ElementAt(index).Key;
    
    public bool Contains(TK key) => Nodes.Any(node => node.Key.Equals(key));
    
    public Node<TK>[] ToArray() => Nodes.ToArray();

    public Node<TK> this[TK key]
    {
        get
        {
            using var enumerator = Nodes.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Key.Equals(key))
                    return enumerator.Current;
            }
            
            throw new InvalidOperationException();
        }
    }
    
    public Node<TK> RandomNode() => Nodes.ElementAt(_random.Next(NodesCount));
    
    public IEnumerator<Node<TK>> GetEnumerator()
    {
        return Nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}