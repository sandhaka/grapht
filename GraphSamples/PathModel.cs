using GraphT.Models.Abstractions;

namespace GraphSamples;

public class PathModel : IGraphListModel<string>
{
    public IDictionary<string, List<(string Value, decimal Cost)>> AdjacencyList { get; } =
        new Dictionary<string, List<(string Value, decimal Cost)>>
        {
            ["A"] = [("B", 3), ("F", 7)],
            ["B"] = [("C", 2), ("H", 8)],
            ["F"] = [("G", 1)],
            ["C"] = [("F", 1)],
            ["H"] = [("K", 1)],
            ["G"] = [("H", 9)],
            ["K"] = [("Z", 0)],
            ["Z"] = []
        };
}