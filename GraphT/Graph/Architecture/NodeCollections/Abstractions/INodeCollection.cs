using GraphT.Graph.Architecture.Components;

namespace GraphT.Graph.Architecture.NodeCollections.Abstractions;

internal interface INodeCollection<TK> where TK : IEquatable<TK>
{
    IReadOnlySet<TK> Keys { get; }
    int NodesCount { get; }
    TK Key(int index);
    bool Contains(TK key);
    Node<TK>[] ToArray();
    Node<TK> this[TK key] { get; }
}