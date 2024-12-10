using System.Collections.Frozen;
using System.Diagnostics;
using GraphT.Graph.Architecture.Components;

namespace GraphT.Graph.Architecture.NodeCollections;

[DebuggerDisplay("{NodesCount} nodes")]
internal sealed class FrozenNodeCollection<T> : NodesCollectionBase<T>
    where T : IEquatable<T>
{
    public FrozenNodeCollection(IEnumerable<Node<T>> nodes) : base(nodes.ToFrozenSet())
    {
    }
}