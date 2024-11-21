using GraphT.Graph.Architecture.Components;

namespace GraphT.Graph.Architecture.Abstractions;

internal interface IGraphComponents<T> 
    where T : IEquatable<T>
{
    IReadOnlySet<T> NodeValues { get; }
    Node<T> this[T value] { get; }
}