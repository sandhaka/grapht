using GraphT.Graph.Exceptions;
using GraphT.Models.Abstractions;

namespace GraphT.Models.Validators;

internal static class GraphModelValidatorsExtensions
{
    public static void Validate<T>(this IGraphListModel<T> model) where T : IEquatable<T>
    {
        // Check if all edges are valid
        var edges = model.AdjacencyList;
        foreach (var edge in edges)
        {
            if (edge.Value.Any(ev => !edges.ContainsKey(ev.Value)))
                throw new InvalidGraphDataException<T>(edge.Key);
        }
    }
    
    public static void Validate<T>(this IGraphMatrixModel<T> model) where T : IEquatable<T>
    {
        // Check first two dimensions
        if (model.Matrix.GetLength(1) != model.Nodes.Length)
            throw new InvalidGraphDataException<T>();
        if (model.Matrix.GetLength(0) != model.Nodes.Length)
            throw new InvalidGraphDataException<T>();
    }
}