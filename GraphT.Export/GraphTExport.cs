using GraphT.Graph;

namespace GraphT.Export;

public static class GraphTExport
{
    /// <summary>
    /// Exports the graph to the specified output format.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the graph nodes.</typeparam>
    /// <param name="graph">The graph instance to be exported.</param>
    /// <param name="output">The output format for the export. Defaults to GraphExportOutput.Plain.</param>
    /// <returns>A string representation of the graph in the specified output format.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified <paramref name="output"/> is not a valid GraphExportOutput value.
    /// </exception>
    public static string Export<T>(this IGraph<T> graph, GraphExportOutput output = GraphExportOutput.Plain)
        where T : IEquatable<T>
    {
        return output switch
        {
            GraphExportOutput.Plain => new ExportToPlain<T>(graph).Export(),
            GraphExportOutput.Gephi => new ExportToGephi<T>(graph).Export(),
            _ => throw new ArgumentOutOfRangeException(nameof(output), output, null)
        };
    }
}