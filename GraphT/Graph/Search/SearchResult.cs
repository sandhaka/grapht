namespace GraphT.Graph.Search;

/// <summary>
/// Represents the result of a graph search operation, including
/// the path found and its associated total cost.
/// </summary>
/// <typeparam name="TK">
/// The type of the nodes in the graph.
/// </typeparam>
public class SearchResult<TK>
{
    /// <summary>
    /// Gets the collection of nodes representing the path identified during the graph search operation.
    /// The path is represented as an enumerable of nodes of the specified type.
    /// </summary>
    /// <typeparam name="TK">
    /// The type of the nodes in the graph.
    /// </typeparam>
    public required IEnumerable<TK> Path { get; init; }

    /// <summary>
    /// Gets the total cost associated with the path identified during the graph search operation.
    /// The total cost represents the cumulative weight or distance of the discovered path.
    /// </summary>
    public required decimal TotalCost { get; init; }
}