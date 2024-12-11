using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GraphT.Graph;
using GraphT.Graph.Search;
using GraphT.Graph.Search.Strategies;
using ProblemSamples;

namespace Benchmarks.PerfCase;

[MemoryDiagnoser]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net80)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net90)]
public class BacktrackingPathFinding
{
    private readonly IPathSearch<string> _pathSearch;

    public BacktrackingPathFinding()
    {
        var largePathFindingProblem = new LargeGraphProblem();
        var graph = Graph<string>.CreateReadOnly(largePathFindingProblem);
        _pathSearch = graph.ToPathSearch(new Backtracking<string>());
    }
    
    [Benchmark(Description = "Simple Backtracking Path Finding")]
    public bool SubmitBenchmark()
    {
        bool result = false;
        for (var i = 0; i < 1000; i++)
            result = _pathSearch.Search("Node1", "Node100", out _);

        return result;
    }
}