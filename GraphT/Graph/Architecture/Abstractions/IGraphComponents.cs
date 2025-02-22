using GraphT.Graph.Architecture.Components;

namespace GraphT.Graph.Architecture.Abstractions;

internal interface IGraphComponents<TK> 
    where TK : IEquatable<TK>
{
    IReadOnlySet<TK> NodeKeys { get; }
    Node<TK> this[TK key] { get; }
}