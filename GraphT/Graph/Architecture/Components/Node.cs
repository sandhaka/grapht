using System.Diagnostics;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Components;

[DebuggerDisplay("{Value}")]
internal record Node<T>(T Value) where T : IEquatable<T>
{
    private ValueOption<Memory<Edge<T>>> _neighbors = ValueOption<Memory<Edge<T>>>.None();
    
    public Memory<Edge<T>> Edges
    {
        get => _neighbors.Reduce(Memory<Edge<T>>.Empty);
        set => _neighbors = value.IsEmpty
            ? ValueOption<Memory<Edge<T>>>.None()
            : ValueOption<Memory<Edge<T>>>.Some(value);
    }
    
    public bool HasEdges => !Edges.IsEmpty;

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public virtual bool Equals(Node<T>? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (ReferenceEquals(null, other)) return false;
        return Value.Equals(other.Value);
    }
}