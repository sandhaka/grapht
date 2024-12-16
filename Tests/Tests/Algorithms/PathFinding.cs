using GraphT.Graph;
using GraphT.Graph.Search;
using GraphT.Graph.Search.Context;
using GraphT.Graph.Search.Strategies;
using GraphT.Problems.Abstractions;
using ProblemSamples;
using Xunit.Abstractions;

namespace Tests.Tests.Algorithms;

public class PathFinding(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);

    private IPathSearch<TNode> PreparePathSearch<TNode>(IGraphProblem<TNode> problem, IShortestPathSearchStrategy<TNode> searchStrategy) where TNode : IEquatable<TNode>
    {
        var graph = Graph<TNode>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch(searchStrategy);
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        return pathSearch;
    }
    
    private IPathSearch<TNode> PreparePathSearch<TNode>(IGraphProblem<TNode> problem) where TNode : IEquatable<TNode>
    {
        var graph = Graph<TNode>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch();
        
        output.WriteLine($"Using {pathSearch.ShortestPathSearchStrategy.Name} search strategy.");
        
        return pathSearch;
    }
    
    [Fact]
    public void ShouldFindShortestPathWithDijkstra()
    {
        var problem = new PathProblem();
        var pathSearch = PreparePathSearch(problem);
        
        // Act
        var s = pathSearch.Search("A", "Z", out var result);

        var resultPath = result.GetPath(out var finalCost);
        
        output.WriteLine($"Reached {resultPath} with cost {finalCost}");
        
        // Verify
        Assert.True(s);
        Assert.Equal("A,B,H,K,Z", resultPath);
        Assert.Equal(12, finalCost);
    }
    
    [Fact]
    public void ShouldFindShortestPathWithDijkstraEdgeCaseStartIsTarget()
    {
        var problem = new PathProblem();
        var pathSearch = PreparePathSearch(problem);
        
        // Act
        var s = pathSearch.Search("A", "A", out var result);

        var resultPath = result.GetPath(out var finalCost);
        
        // Verify
        Assert.True(s);
        Assert.Equal("A", resultPath);
        Assert.Equal(0, finalCost);
    }
    
    [Fact]
    public void ShouldFindShortestPathWithDijkstraTargetNotExists()
    {
        var problem = new PathProblem();
        var pathSearch = PreparePathSearch(problem);
        
        // Act
        var s = pathSearch.Search("A", "U", out var result);

        var resultPath = result.GetPath(out _);
        
        // Verify
        Assert.False(s);
        Assert.True(string.IsNullOrEmpty(resultPath));
    }

    [Fact]
    public void ShouldFindShortestPathUsingBacktracking()
    {
        var problem = new PathProblem();
        var searchStrategy = new Backtracking<string>();
        var pathSearch = PreparePathSearch(problem, searchStrategy);
        
        // Act
        var s = pathSearch.Search("A", "Z", out var result);
        
        var resultPath = result.GetPath(out decimal finalCost);

        output.WriteLine($"Reached {resultPath} with cost {finalCost}");

        // Verify
        Assert.IsType<Backtracking<string>>(pathSearch.ShortestPathSearchStrategy);
        Assert.True(s);
        Assert.Equal("A,B,H,K,Z", resultPath);  
        Assert.Equal(12, finalCost);
    }
    
    [Fact]
    public void ShouldFindShortestPathWithBacktrackingEdgeCaseStartIsTarget()
    {
        var problem = new PathProblem();
        var searchStrategy = new Backtracking<string>();
        var pathSearch = PreparePathSearch(problem, searchStrategy);
        
        // Act
        var s = pathSearch.Search("A", "A", out var result);

        var resultPath = result.GetPath(out var finalCost);
        
        output.WriteLine($"Reached {resultPath} with cost {finalCost}");
        
        // Verify
        Assert.IsType<Backtracking<string>>(pathSearch.ShortestPathSearchStrategy);
        Assert.True(s);
        Assert.Equal("A", resultPath);
        Assert.Equal(0, finalCost);
    }
    
    [Fact]
    public void ShouldFindShortestPathWithBacktrackingTargetNotExists()
    {
        var problem = new PathProblem();
        var searchStrategy = new Backtracking<string>();
        var pathSearch = PreparePathSearch(problem, searchStrategy);
        
        // Act
        var s = pathSearch.Search("A", "U", out var result);

        var resultPath = result.GetPath(out _);
        
        // Verify
        Assert.IsType<Backtracking<string>>(pathSearch.ShortestPathSearchStrategy);
        Assert.False(s);
        Assert.True(string.IsNullOrEmpty(resultPath));
    }
    
    [Fact]
    public void ShouldFindShortestPathWithAStar()
    {
        var pathFindingOfRomanianCities = new RomaniaMapGraphProblem();
        var searchStrategy = new AStar<GeoNodeValue>(Heuristic);
        var pathSearch = PreparePathSearch(pathFindingOfRomanianCities, searchStrategy);
        
        var startNode = pathFindingOfRomanianCities.Get("Arad");
        var endNode = pathFindingOfRomanianCities.Get("Bucharest");
        
        // Act
        var s = pathSearch.Search(startNode, endNode, out var result);
        
        var reducedResult = result.Reduce(new SearchResult<GeoNodeValue> { Path = new List<GeoNodeValue>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path.Select(n => n.Name));
        output.WriteLine($"Reached {resultPath} with cost {reducedResult.TotalCost}");
        
        // Verify
        Assert.IsType<AStar<GeoNodeValue>>(pathSearch.ShortestPathSearchStrategy);
        Assert.True(s);
        Assert.Equal("Arad,Sibiu,Rimnicu,Pitesti,Bucharest", resultPath);
        Assert.Equal(418, reducedResult.TotalCost);
        return;

        // Define a valid heuristic function for the problem
        decimal Heuristic(GeoNodeValue node, IPathSearchContext<GeoNodeValue> context)
        {
            var target = context.Target;
            var x = node.X - target.X;
            var y = node.Y - target.Y;
            return (decimal) Math.Sqrt(x * x + y * y);
        }
    }

    [Fact]
    public void ShouldFindShortestPathInLargeGraphUsedInBenchmarks()
    {
        var problem = new LargeGraphProblem();
        var pathSearch = PreparePathSearch(problem);
        
        // Act
        var s = pathSearch.Search("Node1", "Node100", out _);
        
        // Verify
        Assert.True(s);
    }
}