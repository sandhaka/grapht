using Microsoft.Extensions.ObjectPool;
using System.Diagnostics.CodeAnalysis;
using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Dto;
using GraphT.Graph.Search.Context;

namespace GraphT.Graph.Architecture.Search;

[method: SetsRequiredMembers]
internal class PathSearchContext<TK>(IGraphComponents<TK> graphComponents, TK start, TK target) : IPathSearchContext<TK>
    where TK : IEquatable<TK>
{
    private static readonly ObjectPool<EdgeTuple<TK>> EdgeTuplePool =
        new DefaultObjectPool<EdgeTuple<TK>>(new DefaultPooledObjectPolicy<EdgeTuple<TK>>());

    public IReadOnlySet<TK> NodeKeys => graphComponents.NodeKeys;

    public IEnumerable<EdgeTuple<TK>> NodeEdges(TK nodeKey)
    {
        var node = graphComponents[nodeKey];

        if (!node.HasEdges)
            yield break;

        var nodeEdges = node.Edges;

        for (var i = 0; i < nodeEdges.Length; i++)
        {
            var edge = nodeEdges.Span[i];
            var tuple = EdgeTuplePool.Get();

            tuple.NodeKey = edge.To.Key;
            tuple.Cost = edge.Cost;

            yield return tuple;

            EdgeTuplePool.Return(tuple);
        }
    }

    public required TK Start { get; init; } = start;
    public required TK Target { get; init; } = target;
}