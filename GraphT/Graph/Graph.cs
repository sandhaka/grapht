using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.Implementations;
using GraphT.Models.Abstractions;
using GraphT.Models.Validators;

namespace GraphT.Graph;

/// <summary>
/// Provides static methods related to creating graphs.
/// </summary>
/// <typeparam name="TK">The type of the elements in the graph, typically vertices or nodes.</typeparam>
public static class Graph<TK> 
    where TK : IEquatable<TK>
{
    /// <summary>
    /// Creates a read-only graph from the given model.
    /// </summary>
    /// <param name="listModel">The graph model containing the adjacency list</param>
    /// <returns>A read-only graph constructed from the model</returns>
    public static IGraph<TK> CreateReadOnly(IGraphListModel<TK> listModel)
    {
        listModel.Validate();
        var nodes = CreateNodes(listModel);
        var graph = ReadOnlyGraph<TK>.Create(nodes);
        
        return graph;
    }

    /// <summary>
    /// Creates a read-only graph from the specified adjacency matrix model.
    /// </summary>
    /// <param name="matrixModel">The graph model containing the adjacency matrix and associated node keys.</param>
    /// <returns>A read-only graph constructed from the adjacency matrix model.</returns>
    public static IGraph<TK> CreateReadOnly(IGraphMatrixModel<TK> matrixModel)
    {
        matrixModel.Validate();
        var matrix = matrixModel.Matrix;
        var nodeKeys = matrixModel.Nodes;
        var nodes = ParseMatrix(matrix, nodeKeys);
        var graph = ReadOnlyGraph<TK>.Create(nodes);
        
        return graph;
    }

    private static HashSet<Node<TK>> ParseMatrix(decimal[,] matrix, TK[] nodeKeys)
    {
        var set = new HashSet<Node<TK>>();
        var nodes = nodeKeys.ToDictionary(v => v, v => new Node<TK>(v)); // Map keys to nodes
        var weights = nodeKeys.ToDictionary(v => v, v => 0m); // Temporary storing weights
        
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var w = matrix[i, j];
                if (w == 0) continue; // No connected
                weights[nodeKeys[j]] = w;
            }

            var node = nodes[nodeKeys[i]];

            var edges = weights
                .Where(w => w.Value > 0)
                .Select(e => new Edge<TK>(node, nodes[e.Key], e.Value))
                .ToArray();
            
            node.Edges = new Memory<Edge<TK>>(edges);
            
            set.Add(node); 
            
            weights.Clear();
        }

        return set;
    }

    private static HashSet<Node<TK>> CreateNodes(IGraphListModel<TK> listModel)
    {
        var nodes = listModel.AdjacencyList.Keys
            .Select(k => new Node<TK>(k))
            .ToHashSet();
        
        SetNeighborhoods(nodes, listModel);
        
        return nodes;
    }
    
    private static void SetNeighborhoods(HashSet<Node<TK>> nodes, IGraphListModel<TK> listModel)
    {
        for (var i = 0; i < listModel.AdjacencyList.Count; i++)
        {
            var (entryKey, neighbor) = listModel.AdjacencyList.ElementAt(i);
            
            var node = nodes.Single(n => n.Key.Equals(entryKey));
            
            var edges = neighbor
                .Select(x => new Edge<TK>(node, nodes.Single(n => n.Key.Equals(x.Key)), x.Cost))
                .ToArray();
            
            node.Edges = new Memory<Edge<TK>>(edges);
        }
    }
}