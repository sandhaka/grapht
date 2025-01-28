using GraphT.Graph.Exceptions;
using GraphT.Models.Abstractions;

namespace GraphT.Models.Validators;

public static class GraphModelValidatorsExtensions
{
    internal static void Validate<T>(this IGraphListModel<T> model) where T : IEquatable<T>
    {
        // Check if all edges are valid
        var edges = model.AdjacencyList;
        foreach (var edge in edges)
        {
            if (edge.Value.Any(ev => !edges.ContainsKey(ev.Value)))
                throw new InvalidGraphDataException<T>(edge.Key);
        }
    }
    
    internal static void Validate<T>(this IGraphMatrixModel<T> model) where T : IEquatable<T>
    {
        // TODO
    }
}