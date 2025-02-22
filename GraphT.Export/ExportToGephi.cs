using System.Globalization;
using System.Text;
using GraphT.Graph;

namespace GraphT.Export;

internal class ExportToGephi<TK>(IGraph<TK> graph) where TK : IEquatable<TK>
{
    public string Export()
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.AppendLine("Source,Target,Type,Id,Label,Weight");

        var edges = graph.AllEdges();
        
        var id = 0;
        foreach (var (from, cost, to) in edges)
            stringBuilder.AppendLine($"{from},{to},Directed,{id++},,{cost.ToString("F", CultureInfo.InvariantCulture)}");
        
        return stringBuilder.ToString();
    }
}