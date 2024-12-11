using Microsoft.Extensions.ObjectPool;
using System.Diagnostics.CodeAnalysis;
using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Search.Context;

namespace GraphT.Graph.Architecture.Implementations;

[method: SetsRequiredMembers]
internal class PathSearchContext<T>(IGraphComponents<T> graphComponents, T start, T target) : IPathSearchContext<T>
    where T : IEquatable<T>
{
    private static readonly ObjectPool<EdgeTuple<T>> EdgeTuplePool =
        new DefaultObjectPool<EdgeTuple<T>>(new DefaultPooledObjectPolicy<EdgeTuple<T>>());

    public IReadOnlySet<T> NodeValues => graphComponents.NodeValues;

    public IEnumerable<EdgeTuple<T>> NodeEdges(T nodeValue)
    {
        var node = graphComponents[nodeValue];

        if (!node.HasEdges)
            yield break;

        var nodeEdges = node.Edges;

        for (var i = 0; i < nodeEdges.Length; i++)
        {
            var edge = nodeEdges.Span[i];
            var tuple = EdgeTuplePool.Get();

            tuple.NodeValue = edge.To.Value;
            tuple.Cost = edge.Cost;

            yield return tuple;

            EdgeTuplePool.Return(tuple);
        }
    }

    public required T Start { get; init; } = start;
    public required T Target { get; init; } = target;
}