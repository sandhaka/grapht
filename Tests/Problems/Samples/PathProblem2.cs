using GraphT.Problems.Abstractions;

namespace Tests.Problems.Samples;

public class PathProblem2 : IGraphProblem<byte>
{
    public IDictionary<byte, List<(byte Value, decimal Cost)>> AdjacencyList { get; } =
        new Dictionary<byte, List<(byte Value, decimal Cost)>>
        {
            [0] = [(1, 2), (2, 1)],
            [1] = [(2, 3), (3, 4)],
            [2] = [(3, 1)],
            [3] = [(0, 5)],
            [4] = [(1, 2), (3, 3)]
        };
}