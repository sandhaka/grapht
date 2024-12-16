using GraphT.Graph.Architecture.Components;

namespace GraphT.Graph.Architecture.NodeCollections.Abstractions;

internal interface INodeCollection<T> where T : IEquatable<T>
{
    IReadOnlySet<T> Values { get; }
    int NodesCount { get; }
    T Value(int index);
    bool Contains(T value);
    Node<T>[] ToArray();
    Node<T> this[T value] { get; }
}