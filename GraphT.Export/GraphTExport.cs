using GraphT.Graph;

namespace GraphT.Export;

public static class GraphTExport
{
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