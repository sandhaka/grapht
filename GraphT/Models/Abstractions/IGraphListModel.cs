namespace GraphT.Models.Abstractions;

/// <summary>
/// Represents a generic graph model using an adjacency list.
/// </summary>
/// <typeparam name="T">The type of the elements in the graph, typically vertices or nodes.</typeparam>
public interface IGraphListModel<T> where T : IEquatable<T>
{
    /// <summary>
    /// Gets the adjacency list representing the graph. The adjacency list is a dictionary
    /// where each key is a node, and the value is a collection of tuples. Each tuple contains
    /// a neighboring node and the cost to reach that neighbor from the key node.
    /// </summary>
    /// <value>
    /// A dictionary where the key is a node of type <typeparamref name="T"/>, and the value is
    /// an list of tuples, each containing a node of type <typeparamref name="T"/> and a
    /// decimal representing the cost.
    /// </value>
    public IDictionary<T, List<(T Value, decimal Cost)>> AdjacencyList { get; }
}