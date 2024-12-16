using System.Diagnostics;

namespace GraphT.Graph.Architecture.Components;

[DebuggerDisplay("{Value}")]
internal record Node<T>(T Value) where T : IEquatable<T>
{
    public Memory<Edge<T>> Edges { get; set; } = Memory<Edge<T>>.Empty;
    
    public bool HasEdges => !Edges.IsEmpty;

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public virtual bool Equals(Node<T>? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (ReferenceEquals(null, other)) return false;
        return Value.Equals(other.Value);
    }
}