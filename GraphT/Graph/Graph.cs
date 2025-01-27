using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.Implementations;
using GraphT.Graph.Exceptions;
using GraphT.Problems.Abstractions;

namespace GraphT.Graph;

/// <summary>
/// Provides static methods related to creating graphs.
/// </summary>
/// <typeparam name="T">The type of the elements in the graph, typically vertices or nodes.</typeparam>
public static class Graph<T> 
    where T : IEquatable<T>
{
    /// <summary>
    /// Creates a read-only graph from the given problem.
    /// </summary>
    /// <param name="problem">The graph problem containing the adjacency list</param>
    /// <returns>A read-only graph constructed from the problem</returns>
    public static IGraph<T> CreateReadOnly(IGraphProblem<T> problem)
    {
        CheckProblem(problem);
        var nodes = CreateNodes(problem);
        var graph = ReadOnlyGraph<T>.Create(nodes);
        
        return graph;
    }

    public static IGraph<T> CreateReadOnly(decimal[,] matrix, T[] nodeValues)
    {
        var nodes = ParseMatrix(matrix, nodeValues).ToHashSet();
        var graph = ReadOnlyGraph<T>.Create(nodes);
        
        return graph;
    }

    private static IEnumerable<Node<T>> ParseMatrix(decimal[,] matrix, T[] nodeValues)
    {
        var weights = nodeValues.ToDictionary(v => v, v => 0m);
        
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                var w = matrix[i, j];
                if (w == 0) continue;
                weights[nodeValues[j]] = w;
            }

            var edges = weights
                .Where(w => w.Value > 0)
                .Select(e => new Edge<T>(new Node<T>(e.Key), e.Value))
                .ToArray();
            
            var node = new Node<T>(nodeValues[i])
            {
                Edges = new Memory<Edge<T>>(edges)
            };
            yield return node; 
            
            weights.Clear();
        }
    }

    private static HashSet<Node<T>> CreateNodes(IGraphProblem<T> problem)
    {
        var nodes = problem.AdjacencyList.Keys
            .Select(v => new Node<T>(v))
            .ToHashSet();
        
        SetNeighborhoods(nodes, problem);
        
        return nodes;
    }
    
    private static void SetNeighborhoods(HashSet<Node<T>> nodes, IGraphProblem<T> problem)
    {
        for (var i = 0; i < problem.AdjacencyList.Count; i++)
        {
            var (entryKey, neighborValues) = problem.AdjacencyList.ElementAt(i);
            
            var node = nodes.Single(n => n.Value.Equals(entryKey));
            
            var edges = neighborValues
                .Select(x => new Edge<T>(nodes.Single(n => n.Value.Equals(x.Value)), x.Cost))
                .ToArray();
            
            node.Edges = new Memory<Edge<T>>(edges);
        }
    }

    private static void CheckProblem(IGraphProblem<T> problem)
    {
        // Check if all edges are valid
        var edges = problem.AdjacencyList;
        foreach (var edge in edges)
        {
            if (edge.Value.Any(ev => !edges.ContainsKey(ev.Value)))
                throw new InvalidGraphDataException<T> { Item = edge.Key };
        }
    }
}