using GraphT.Problems.Abstractions;

namespace ProblemSamples;

public class SimpleGraphProblem : IGraphProblem<string>
{
    public IDictionary<string, List<(string Value, decimal Cost)>> AdjacencyList { get; } = 
        new Dictionary<string, List<(string Value, decimal Cost)>> 
        {
            ["A"] = [("B", 0), ("C", 0)],
            ["B"] = [("D", 0)],
            ["C"] = [("K", 0)],
            ["D"] = [("E", 0)],
            ["E"] = [("F", 0)],
            ["F"] = [("G", 0)],
            ["G"] = [("H", 0)],
            ["H"] = [("I", 0)],
            ["I"] = [("J", 0)],
            ["J"] = [],
            ["K"] = [("L", 0)],
            ["L"] = [("M", 0), ("N", 0)],
            ["M"] = [],
            ["N"] = []
        };
}