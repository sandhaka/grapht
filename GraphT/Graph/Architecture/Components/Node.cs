using System.Diagnostics;

namespace GraphT.Graph.Architecture.Components;

[DebuggerDisplay("{Key}")]
internal record Node<TK>(TK Key) where TK : IEquatable<TK>
{
    public Memory<Edge<TK>> Edges { get; set; } = Memory<Edge<TK>>.Empty;
    
    public bool HasEdges => !Edges.IsEmpty;

    public override int GetHashCode() => Key?.GetHashCode() ?? 0;

    public virtual bool Equals(Node<TK>? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (ReferenceEquals(null, other)) return false;
        return Key.Equals(other.Key);
    }
}