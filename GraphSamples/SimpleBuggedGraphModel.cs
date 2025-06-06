using GraphT.Models.Abstractions;

namespace GraphSamples;

public class SimpleBuggedGraphModel : IGraphListModel<string>
{
    public IDictionary<string, List<(string Key, decimal Cost)>> AdjacencyList { get; } = 
        new Dictionary<string, List<(string Key, decimal Cost)>>
        {
            ["A"] = [("B", 1m), ("C", 1m)],
            ["B"] = [("D", 1m)],
            ["C"] = [("K", 1m)],
            ["D"] = [("E", 1m)],
            ["E"] = [("F", 1m)],
            ["F"] = [("G", 1m)],
            ["G"] = [("H", 1m)],
            ["H"] = [("I", 1m)],
            ["I"] = [("J", 1m)],
            ["J"] = [],
            ["K"] = [("L", 1m)],
            ["L"] = [("M", 1m), ("N", 1m)],
            ["N"] = []
        };
}