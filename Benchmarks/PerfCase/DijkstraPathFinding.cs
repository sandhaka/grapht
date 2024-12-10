using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GraphT.Graph;
using GraphT.Graph.Search;
using ProblemSamples;

namespace Benchmarks.PerfCase;

[MemoryDiagnoser] // Enables memory allocation diagnostics
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net80)] // Target .NET 8
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net90)] // Target .NET 9
public class DijkstraPathFinding
{
    private readonly IPathSearch<string> _pathSearch;

    public DijkstraPathFinding()
    {
        var largePathFindingProblem = new LargeGraphProblem();
        var graph = Graph<string>.CreateReadOnly(largePathFindingProblem);
        _pathSearch = graph.ToPathSearch();
    }
    
    [Benchmark]
    public bool SubmitBenchmark()
    {
        bool result = false;
        for (var i = 0; i < 1000; i++)
            result = _pathSearch.Search("Node1", "Node100", out _);

        return result;
    }
}