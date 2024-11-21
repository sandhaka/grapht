using System.Diagnostics.CodeAnalysis;
using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Search;

namespace GraphT.Graph.Architecture.Implementations;

[method: SetsRequiredMembers]
internal class PathSearchContext<T>(IGraphComponents<T> graphComponents, T start, T target) : IPathSearchContext<T>
    where T : IEquatable<T>
{
    public IReadOnlySet<T> NodeValues => graphComponents.NodeValues;

    public IEnumerable<(T Value, decimal Cost)> Neighbors(T value)
    {
        foreach (var (node, cost) in graphComponents[value].Neighbors.Span.ToArray())
            yield return (node.Value, cost);
    }

    public required T Start { get; init; } = start;
    public required T Target { get; init; } = target;
}