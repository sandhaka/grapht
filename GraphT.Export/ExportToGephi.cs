using System.Globalization;
using System.Text;
using GraphT.Graph;

namespace GraphT.Export;

internal class ExportToGephi<T>(IGraph<T> graph) where T : IEquatable<T>
{
    public string Export()
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.AppendLine("Source,Target,Weight");

        var edges = graph.AllEdges();
        
        foreach (var (from, cost, to) in edges)
            stringBuilder.AppendLine($"{from},{to},{cost.ToString("F", CultureInfo.InvariantCulture)}");
        
        return stringBuilder.ToString();
    }
}