using System.Collections.Frozen;
using System.Diagnostics;
using GraphT.Graph.Architecture.Components;

namespace GraphT.Graph.Architecture.NodeCollections;

[DebuggerDisplay("{NodesCount} nodes")]
internal sealed class FrozenNodeCollection<TK> : NodesCollectionBase<TK>
    where TK : IEquatable<TK>
{
    public FrozenNodeCollection(IEnumerable<Node<TK>> nodes) : base(nodes.ToFrozenSet())
    {
    }
}