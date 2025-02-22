namespace GraphT.Models.Abstractions;

/// <summary>
/// Represents a model for a graph adjacency matrix that uses generic type nodes.
/// </summary>
/// <typeparam name="TK">The type of nodes in the graph, where TK must implement IEquatable.</typeparam>
public interface IGraphMatrixModel<out TK> where TK : IEquatable<TK>
{
    /// <summary>
    /// Gets the adjacency matrix representation of the graph.
    /// The matrix uses decimal values to define the weighted edges between nodes,
    /// where the value at the position [i, j] represents the weight of the edge from node i to node j.
    /// </summary>
    public decimal[,] Matrix { get; }

    /// <summary>
    /// Gets the collection of nodes in the graph.
    /// Each node is represented by its corresponding key TK, where TK must implement IEquatable.
    /// </summary>
    public TK[] Nodes { get; }
}