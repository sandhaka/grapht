using GraphT.Graph.Architecture.Components;

namespace GraphT.Graph.Architecture.NodeCollections.Abstractions;

internal interface INodeCollection<T> where T : IEquatable<T>
{
    int NodesCount { get; }
    bool Contains(T value);
    Node<T> this[T value] { get; }
    IReadOnlySet<T> Values { get; }
}