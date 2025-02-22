using GraphT.Graph;
using GraphT.Graph.Search;
using GraphT.Graph.Search.Context;
using GraphT.Graph.Search.Strategies;
using GraphT.Models.Abstractions;
using GraphSamples;
using Xunit.Abstractions;

namespace Tests.Tests.Algorithms;

public class PathFinding(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);

    private IPathSearch<TNode> PreparePathSearch<TNode>(IGraphListModel<TNode> listModel, IShortestPathSearchStrategy<TNode> searchStrategy) where TNode : IEquatable<TNode>
    {
        var graph = Graph<TNode>.CreateReadOnly(listModel);
        var pathSearch = graph.ToPathSearch(searchStrategy);
        
        _output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        return pathSearch;
    }
    
    private IPathSearch<TNode> PreparePathSearch<TNode>(IGraphListModel<TNode> listModel) where TNode : IEquatable<TNode>
    {
        var graph = Graph<TNode>.CreateReadOnly(listModel);
        var pathSearch = graph.ToPathSearch();
        
        _output.WriteLine($"Using {pathSearch.ShortestPathSearchStrategy.Name} search strategy.");
        
        return pathSearch;
    }
    
    [Fact]
    public void ShouldFindShortestPathWithDijkstra()
    {
        var problem = new PathModel();
        var pathSearch = PreparePathSearch(problem);
        
        // Act
        var s = pathSearch.Search("A", "Z", out var result);

        var resultPath = result.GetPath(out var finalCost);
        
        _output.WriteLine($"Reached {resultPath} with cost {finalCost}");
        
        // Verify
        Assert.True(s);
        Assert.Equal("A,B,H,K,Z", resultPath);
        Assert.Equal(12, finalCost);
    }
    
    [Fact]
    public void ShouldFindShortestPathWithDijkstraEdgeCaseStartIsTarget()
    {
        var problem = new PathModel();
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
        var problem = new PathModel();
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
        var problem = new PathModel();
        var searchStrategy = new Backtracking<string>();
        var pathSearch = PreparePathSearch(problem, searchStrategy);
        
        // Act
        var s = pathSearch.Search("A", "Z", out var result);
        
        var resultPath = result.GetPath(out decimal finalCost);

        _output.WriteLine($"Reached {resultPath} with cost {finalCost}");

        // Verify
        Assert.IsType<Backtracking<string>>(pathSearch.ShortestPathSearchStrategy);
        Assert.True(s);
        Assert.Equal("A,B,H,K,Z", resultPath);  
        Assert.Equal(12, finalCost);
    }
    
    [Fact]
    public void ShouldFindShortestPathWithBacktrackingEdgeCaseStartIsTarget()
    {
        var problem = new PathModel();
        var searchStrategy = new Backtracking<string>();
        var pathSearch = PreparePathSearch(problem, searchStrategy);
        
        // Act
        var s = pathSearch.Search("A", "A", out var result);

        var resultPath = result.GetPath(out var finalCost);
        
        _output.WriteLine($"Reached {resultPath} with cost {finalCost}");
        
        // Verify
        Assert.IsType<Backtracking<string>>(pathSearch.ShortestPathSearchStrategy);
        Assert.True(s);
        Assert.Equal("A", resultPath);
        Assert.Equal(0, finalCost);
    }
    
    [Fact]
    public void ShouldFindShortestPathWithBacktrackingTargetNotExists()
    {
        var problem = new PathModel();
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
        var pathFindingOfRomanianCities = new RomaniaMapGraphModel();
        var searchStrategy = new AStar<GeoNode>(Heuristic);
        var pathSearch = PreparePathSearch(pathFindingOfRomanianCities, searchStrategy);
        
        var startNode = pathFindingOfRomanianCities.Get("Arad");
        var endNode = pathFindingOfRomanianCities.Get("Bucharest");
        
        // Act
        var s = pathSearch.Search(startNode, endNode, out var result);
        
        var reducedResult = result.Reduce(new SearchResult<GeoNode> { Path = new List<GeoNode>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path.Select(n => n.Name));
        _output.WriteLine($"Reached {resultPath} with cost {reducedResult.TotalCost}");
        
        // Verify
        Assert.IsType<AStar<GeoNode>>(pathSearch.ShortestPathSearchStrategy);
        Assert.True(s);
        Assert.Equal("Arad,Sibiu,Rimnicu,Pitesti,Bucharest", resultPath);
        Assert.Equal(418, reducedResult.TotalCost);
        return;

        // Define a valid heuristic function for the model
        decimal Heuristic(GeoNode node, IPathSearchContext<GeoNode> context)
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
        var problem = new LargeGraphListModel();
        var pathSearch = PreparePathSearch(problem);
        
        // Act
        var s = pathSearch.Search("Node1", "Node100", out _);
        
        // Verify
        Assert.True(s);
    }
}