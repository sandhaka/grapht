using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.Implementations;
using GraphT.Models.Abstractions;
using GraphT.Models.Validators;

namespace GraphT.Graph;

/// <summary>
/// Provides static methods related to creating graphs.
/// </summary>
/// <typeparam name="T">The type of the elements in the graph, typically vertices or nodes.</typeparam>
public static class Graph<T> 
    where T : IEquatable<T>
{
    /// <summary>
    /// Creates a read-only graph from the given model.
    /// </summary>
    /// <param name="listModel">The graph model containing the adjacency list</param>
    /// <returns>A read-only graph constructed from the model</returns>
    public static IGraph<T> CreateReadOnly(IGraphListModel<T> listModel)
    {
        listModel.Validate();
        var nodes = CreateNodes(listModel);
        var graph = ReadOnlyGraph<T>.Create(nodes);
        
        return graph;
    }

    /// <summary>
    /// Creates a read-only graph from the specified adjacency matrix model.
    /// </summary>
    /// <param name="matrixModel">The graph model containing the adjacency matrix and associated node values.</param>
    /// <returns>A read-only graph constructed from the adjacency matrix model.</returns>
    public static IGraph<T> CreateReadOnly(IGraphMatrixModel<T> matrixModel)
    {
        matrixModel.Validate();
        var matrix = matrixModel.Matrix;
        var nodeValues = matrixModel.Nodes;
        var nodes = ParseMatrix(matrix, nodeValues);
        var graph = ReadOnlyGraph<T>.Create(nodes);
        
        return graph;
    }

    private static HashSet<Node<T>> ParseMatrix(decimal[,] matrix, T[] nodeValues)
    {
        var set = new HashSet<Node<T>>();
        var nodes = nodeValues.ToDictionary(v => v, v => new Node<T>(v)); // Map values to nodes
        var weights = nodeValues.ToDictionary(v => v, v => 0m); // Temporary storing weights
        
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var w = matrix[i, j];
                if (w == 0) continue; // No connected
                weights[nodeValues[j]] = w;
            }

            var node = nodes[nodeValues[i]];

            var edges = weights
                .Where(w => w.Value > 0)
                .Select(e => new Edge<T>(node, nodes[e.Key], e.Value))
                .ToArray();
            
            node.Edges = new Memory<Edge<T>>(edges);
            
            set.Add(node); 
            
            weights.Clear();
        }

        return set;
    }

    private static HashSet<Node<T>> CreateNodes(IGraphListModel<T> listModel)
    {
        var nodes = listModel.AdjacencyList.Keys
            .Select(v => new Node<T>(v))
            .ToHashSet();
        
        SetNeighborhoods(nodes, listModel);
        
        return nodes;
    }
    
    private static void SetNeighborhoods(HashSet<Node<T>> nodes, IGraphListModel<T> listModel)
    {
        for (var i = 0; i < listModel.AdjacencyList.Count; i++)
        {
            var (entryKey, neighborValues) = listModel.AdjacencyList.ElementAt(i);
            
            var node = nodes.Single(n => n.Value.Equals(entryKey));
            
            var edges = neighborValues
                .Select(x => new Edge<T>(node, nodes.Single(n => n.Value.Equals(x.Value)), x.Cost))
                .ToArray();
            
            node.Edges = new Memory<Edge<T>>(edges);
        }
    }
}