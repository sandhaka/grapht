using System.Diagnostics.CodeAnalysis;
using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Search;

namespace GraphT.Graph.Architecture.Implementations;

[method: SetsRequiredMembers]
internal class PathSearchContext<T>(IGraphComponents<T> graphComponents, T start, T target) : IPathSearchContext<T>
    where T : IEquatable<T>
{
    public IReadOnlySet<T> NodeValues => graphComponents.NodeValues;

    public IEnumerable<(T Value, decimal Cost)> Neighbors(T nodeValue)
    {
        var node = graphComponents[nodeValue];

        if (!node.HasEdges)
            yield break;
        
        (T nodeValue, decimal cost) tuple = new();
        
        var nodeEdges = node.Edges;
        
        for (var i = 0; i < nodeEdges.Length; i++)
        {
            var span = nodeEdges.Span[i];
            
            tuple.nodeValue = span.To.Value;
            tuple.cost = span.Cost;
            
            yield return tuple;
        }
    }

    public required T Start { get; init; } = start;
    public required T Target { get; init; } = target;
}