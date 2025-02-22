using GraphT.Graph;

namespace GraphT.Export;

public static class GraphTExport
{
    /// <summary>
    /// Exports the graph to the specified output format.
    /// </summary>
    /// <typeparam name="TK">The key type of the graph nodes.</typeparam>
    /// <param name="graph">The graph instance to be exported.</param>
    /// <param name="output">The output format for the export. Defaults to GraphExportOutput.Plain.</param>
    /// <returns>A string representation of the graph in the specified output format.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified <paramref name="output"/> is not a valid GraphExportOutput value.
    /// </exception>
    public static string Export<TK>(this IGraph<TK> graph, GraphExportOutput output = GraphExportOutput.Plain)
        where TK : IEquatable<TK>
    {
        return output switch
        {
            GraphExportOutput.Plain => new ExportToPlain<TK>(graph).Export(),
            GraphExportOutput.Gephi => new ExportToGephi<TK>(graph).Export(),
            _ => throw new ArgumentOutOfRangeException(nameof(output), output, null)
        };
    }
}