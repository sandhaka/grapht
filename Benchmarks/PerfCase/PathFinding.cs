using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GraphSamples;
using GraphT.Graph;
using GraphT.Graph.Search;
using GraphT.Graph.Search.Strategies;

namespace Benchmarks.PerfCase;

[MemoryDiagnoser]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net80)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net90)]
public class PathFinding
{
    private readonly IGraph<string> _graph;
    
    private IPathSearch<string>? _pathSearch;

    public PathFinding()
    {
        var largePathFindingProblem = new LargeGraphListModel();
        _graph = Graph<string>.CreateReadOnly(largePathFindingProblem);
    }

    [Benchmark(Baseline = true)]
    public bool BackTracking()
    {
        _pathSearch = _graph.ToPathSearch(new Backtracking<string>());
        
        bool result = false;
        for (var i = 0; i < 1000; i++)
            result = _pathSearch.Search("Node1", "Node100", out _);

        return result;
    }

    [Benchmark]
    public bool Dijkstra()
    {
        _pathSearch = _graph.ToPathSearch();
        
        bool result = false;
        for (var i = 0; i < 1000; i++)
            result = _pathSearch.Search("Node1", "Node100", out _);

        return result;
    }
}