namespace GraphT.Models.Abstractions;

/// <summary>
/// Represents a generic graph model using an adjacency list.
/// </summary>
/// <typeparam name="TK">The type of the elements in the graph, typically vertices or nodes.</typeparam>
public interface IGraphListModel<TK> where TK : IEquatable<TK>
{
    /// <summary>
    /// Gets the adjacency list representing the graph. The adjacency list is a dictionary
    /// where each key is a node, and the value is a collection of tuples. Each tuple contains
    /// a neighboring node and the cost to reach that neighbor from the key node.
    /// </summary>
    public IDictionary<TK, List<(TK Key, decimal Cost)>> AdjacencyList { get; }
}