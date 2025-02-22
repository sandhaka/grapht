using GraphT.Graph.Exceptions;
using GraphT.Models.Abstractions;

namespace GraphT.Models.Validators;

internal static class GraphModelValidatorsExtensions
{
    public static void Validate<TK>(this IGraphListModel<TK> model) where TK : IEquatable<TK>
    {
        // Check if all edges are valid
        var edges = model.AdjacencyList;
        foreach (var edge in edges)
        {
            if (edge.Value.Any(ev => !edges.ContainsKey(ev.Key)))
                throw new InvalidGraphDataException<TK>(edge.Key);
        }
    }
    
    public static void Validate<TK>(this IGraphMatrixModel<TK> model) where TK : IEquatable<TK>
    {
        // Check first two dimensions
        if (model.Matrix.GetLength(1) != model.Nodes.Length)
            throw new InvalidGraphDataException<TK>();
        if (model.Matrix.GetLength(0) != model.Nodes.Length)
            throw new InvalidGraphDataException<TK>();
    }
}