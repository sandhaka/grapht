using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GraphT.Graph;
using GraphT.Graph.Search;
using GraphSamples;

namespace Benchmarks.PerfCase;

[MemoryDiagnoser]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net80)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net90)]
public class DijkstraPathFinding
{
    private readonly IPathSearch<string> _pathSearch;

    public DijkstraPathFinding()
    {
        var largePathFindingProblem = new LargeGraphListModel();
        var graph = Graph<string>.CreateReadOnly(largePathFindingProblem);
        _pathSearch = graph.ToPathSearch();
    }
    
    [Benchmark(Description = "Dijkstra Path Finding")]
    public bool SubmitBenchmark()
    {
        bool result = false;
        for (var i = 0; i < 1000; i++)
            result = _pathSearch.Search("Node1", "Node100", out _);

        return result;
    }
}

/* - last run - TODO: move to report or README file
| Method                  | Job      | Runtime  | Mean     | Error    | StdDev   | Gen0      | Gen1     | Allocated |
|------------------------ |--------- |--------- |---------:|---------:|---------:|----------:|---------:|----------:|
| 'Dijkstra Path Finding' | .NET 8.0 | .NET 8.0 | 58.93 ms | 0.263 ms | 0.233 ms | 3888.8889 |        - |  31.09 MB |
| 'Dijkstra Path Finding' | .NET 9.0 | .NET 9.0 | 55.96 ms | 0.330 ms | 0.276 ms | 3888.8889 | 111.1111 |  31.09 MB |
*/