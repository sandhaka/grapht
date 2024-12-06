using GraphT;
using GraphT.Graph;
using GraphT.Graph.Exceptions;
using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using GraphT.Graph.Search.Strategies;
using Tests.Problems.Samples;
using Xunit.Abstractions;

namespace Tests;

public class GraphTests(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);

    [Fact(DisplayName = "Should build a read only graph from a encoded problem.")]
    public void ShouldBuildReadOnlyGraph()
    {
        var problem = new SimpleGraphProblem();

        var graph = Graph<string>.CreateReadOnly(problem);

        Assert.Equal(problem.AdjacencyList.Keys.Count, graph.NodeValues.Count);
    }

    [Fact(DisplayName = "Should fail to build a read only graph from a encoded problem.")]
    public void ShouldFailBuildReadOnlyGraph()
    {
        var problem = new SimpleBuggedGraphProblem();

        Assert.Throws<InvalidGraphDataException<string>>(() => { Graph<string>.CreateReadOnly(problem); });
    }

    [Fact(DisplayName = "Should use an action on graph traversal with DFS.")]
    public void ShouldUseAnActionOnGraphTraversalWithDFS()
    {
        var problem = new SimpleGraphProblem();
        var graph = Graph<string>.CreateReadOnly(problem);

        graph.OnVisitActionParameter = new OnVisit<string>(value =>
        {
            _output.WriteLine($"Visiting {value}");

            if (value.Equals("M"))
                _output.WriteLine("I'm reached M!");
        });

        // Act
        graph.TraverseDfs("A");

        // Verify
        Assert.Contains("I'm reached M!", _output.GetOutput());
    }

    [Fact(DisplayName = "Should use an action on graph traversal with BFS.")]
    public void ShouldUseAnActionOnGraphTraversalWithBFS()
    {
        var problem = new SimpleGraphProblem();
        var graph = Graph<string>.CreateReadOnly(problem);

        graph.OnVisitActionParameter = new OnVisit<string>(value =>
        {
            _output.WriteLine($"Visiting {value}");

            if (value.Equals("M"))
                _output.WriteLine("I'm reached M!");
        });

        // Act
        graph.TraverseBfs("A");

        // Verify
        Assert.Contains("I'm reached M!", _output.GetOutput());
    }
    
    [Fact]
    public void ShouldSearchShortestPathWithDijkstra()
    {
        var problem = new PathProblem();
        var graph = Graph<string>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch();
        var searchStrategy = pathSearch.PathSearchStrategy;
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        // Act
        var s = pathSearch.Search("A", "Z", out var result);

        var reducedResult = result.Reduce(new SearchResult<string>() { Path = new List<string>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path);
        
        output.WriteLine($"Reached {resultPath} with cost {reducedResult.TotalCost}");
        
        // Verify
        Assert.True(s);
        Assert.Equal("A,B,H,K,Z", resultPath);
        Assert.Equal(12, reducedResult.TotalCost);
    }
    
    [Fact]
    public void ShouldSearchShortestPathWithDijkstraEdgeCaseStartIsTarget()
    {
        var problem = new PathProblem();
        var graph = Graph<string>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch();
        var searchStrategy = pathSearch.PathSearchStrategy;
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        // Act
        var s = pathSearch.Search("A", "A", out var result);

        var reducedResult = result.Reduce(new SearchResult<string>() { Path = new List<string>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path);
        
        output.WriteLine($"Reached {resultPath} with cost {reducedResult.TotalCost}");
        
        // Verify
        Assert.True(s);
        Assert.Equal("A", resultPath);
        Assert.Equal(0, reducedResult.TotalCost);
    }
    
    [Fact]
    public void ShouldSearchShortestPathWithDijkstraTargetNotExists()
    {
        var problem = new PathProblem();
        var graph = Graph<string>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch();
        var searchStrategy = pathSearch.PathSearchStrategy;
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        // Act
        var s = pathSearch.Search("A", "U", out var result);

        var reducedResult = result.Reduce(new SearchResult<string>() { Path = new List<string>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path);
        
        // Verify
        Assert.False(s);
        Assert.True(string.IsNullOrEmpty(resultPath));
    }

    [Fact]
    public void ShouldSearchShortestPathUsingBacktracking()
    {
        var problem = new PathProblem();
        var graph = Graph<string>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch(new Backtracking<string>());
        var searchStrategy = pathSearch.PathSearchStrategy;
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        // Act
        var s = pathSearch.Search("A", "Z", out var result);
        
        // Assert the search result
        var reducedResult = result.Reduce(new SearchResult<string>() { Path = new List<string>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path);

        output.WriteLine($"Reached {resultPath} with cost {reducedResult.TotalCost}");

        // Verify
        Assert.True(s);
        Assert.Equal("A,B,H,K,Z", resultPath);  
        Assert.Equal(12, reducedResult.TotalCost);
    }
    
    [Fact]
    public void ShouldSearchShortestPathWithBacktrackingEdgeCaseStartIsTarget()
    {
        var problem = new PathProblem();
        var graph = Graph<string>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch(new Backtracking<string>());
        var searchStrategy = pathSearch.PathSearchStrategy;
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        // Act
        var s = pathSearch.Search("A", "A", out var result);

        var reducedResult = result.Reduce(new SearchResult<string>() { Path = new List<string>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path);
        
        output.WriteLine($"Reached {resultPath} with cost {reducedResult.TotalCost}");
        
        // Verify
        Assert.True(s);
        Assert.Equal("A", resultPath);
        Assert.Equal(0, reducedResult.TotalCost);
    }
    
    [Fact]
    public void ShouldSearchShortestPathWithBacktrackingTargetNotExists()
    {
        var problem = new PathProblem();
        var graph = Graph<string>.CreateReadOnly(problem);
        var pathSearch = graph.ToPathSearch(new Backtracking<string>());
        var searchStrategy = pathSearch.PathSearchStrategy;
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        // Act
        var s = pathSearch.Search("A", "U", out var result);

        var reducedResult = result.Reduce(new SearchResult<string>() { Path = new List<string>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path);
        
        // Verify
        Assert.False(s);
        Assert.True(string.IsNullOrEmpty(resultPath));
    }
    
    [Fact]
    public void ShouldSearchShortestPathWithAStar()
    {
        var problem = new PathProblem3();
        var graph = Graph<GeoNodeValue>.CreateReadOnly(problem);

        var pathSearch = graph.ToPathSearch(new AStar<GeoNodeValue>(Heuristic));
        var searchStrategy = pathSearch.PathSearchStrategy;
        
        output.WriteLine($"Using {searchStrategy.Name} search strategy.");
        
        // Act
        var startNode = problem.Get("Arad");
        var endNode = problem.Get("Bucharest");
        var s = pathSearch.Search(startNode, endNode, out var result);
        
        var reducedResult = result.Reduce(new SearchResult<GeoNodeValue> { Path = new List<GeoNodeValue>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path.Select(n => n.Name));
        output.WriteLine($"Reached {resultPath} with cost {reducedResult.TotalCost}");
        
        // Verify
        Assert.True(s);
        Assert.Equal("Arad,Sibiu,Rimnicu,Pitesti,Bucharest", resultPath);
        Assert.Equal(418, reducedResult.TotalCost);
        return;

        // Define a valid heuristic function for the problem
        decimal Heuristic(GeoNodeValue node)
        {
            var target = problem.Get("Bucharest");
            var x = node.X - target.X;
            var y = node.Y - target.Y;
            return (decimal) Math.Sqrt(x * x + y * y);
        }
    }
}