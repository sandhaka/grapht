using GraphT.Graph;
using GraphT.Graph.Dto;
using GraphT.Graph.Exceptions;
using GraphT.Graph.Parameters;
using Monads.Optional;
using GraphSamples;
using Xunit.Abstractions;

namespace Tests.Tests;

public class Architecture(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);

    [Fact]
    public void ShouldBuildReadOnlyGraph()
    {
        var problem = new SimpleGraphModel();

        var graph = Graph<string>.CreateReadOnly(problem);

        Assert.Equal(problem.AdjacencyList.Keys.Count, graph.NodesCount);
    }

    [Fact]
    public void ShouldFailBuildReadOnlyGraph()
    {
        var problem = new SimpleBuggedGraphModel();

        Assert.Throws<InvalidGraphDataException<string>>(() => { Graph<string>.CreateReadOnly(problem); });
    }

    [Fact]
    public void ShouldUseAnActionOnGraphTraversalWithDFS()
    {
        var problem = new SimpleGraphModel();
        var graph = Graph<string>.CreateReadOnly(problem);

        graph.OnVisitActionParameter = new OnVisit<string>(key =>
        {
            _output.WriteLine($"Visiting {key}");

            if (key.Equals("M"))
                _output.WriteLine("I'm reached M!");
        });

        // Act
        graph.TraverseDfs("A");

        // Verify
        Assert.Contains("I'm reached M!", _output.GetOutput());
    }

    [Fact]
    public void ShouldUseAnActionOnGraphTraversalWithBFS()
    {
        var problem = new SimpleGraphModel();
        var graph = Graph<string>.CreateReadOnly(problem);

        graph.OnVisitActionParameter = new OnVisit<string>(key =>
        {
            _output.WriteLine($"Visiting {key}");

            if (key.Equals("M"))
                _output.WriteLine("I'm reached M!");
        });

        // Act
        graph.TraverseBfs("A");

        // Verify
        Assert.Contains("I'm reached M!", _output.GetOutput());
    }
    
    [Fact]
    public void ShouldDetermineIfAPathExists()
    {
        var problem = new PathModel();
        var graph = Graph<string>.CreateReadOnly(problem);
        
        var areConnected = graph.AreConnected("A", "Z");
        
        Assert.True(areConnected);
    }

    [Fact]
    public void ShouldAddANodeToAGraph()
    {
        var problem = new PathModel();
        var graph = Graph<string>.CreateReadOnly(problem);

        graph.OnVisitActionParameter = new OnVisit<string>(key =>
        {
            _output.WriteLine($"Visiting {key}");

            if (key.Equals("X"))
                _output.WriteLine("X is reachable!");
        });
        
        const string from = "Z";
        const decimal edgeCost = 10.0m;

        EdgeTuple<string>[] ingressEdges = [new() { NodeKey = from, Cost = edgeCost }];

        Assert.False(graph.ContainsNode("X"));
        
        var editableGraph = graph.Mod();

        graph = editableGraph.AddNode("X", ingressEdges, Option<EdgeTuple<string>[]>.None())
            .EndMod();

        graph.TraverseBfs("A");

        Assert.IsAssignableFrom<IGraphMod<string>>(editableGraph);
        Assert.IsAssignableFrom<IGraph<string>>(graph);
        Assert.True(graph.ContainsNode("X"));
        Assert.True(graph.AreConnected("Z", "X"));
        Assert.Contains("X is reachable!", _output.GetOutput());
    }

    [Fact]
    public void ShouldFailToAddAnOrphanNode()
    {
        var problem = new PathModel();
        var graph = Graph<string>.CreateReadOnly(problem);

        var exception1 = Assert.Throws<ArgumentException>(() =>
        {
            graph.Mod().AddNode("X", Option<EdgeTuple<string>[]>.None(), Option<EdgeTuple<string>[]>.None());
        });
        
        Assert.Equal("Trying to add orphan node! Either inEdges or outEdges must be provided", exception1.Message);
    }
    
    [Fact]
    public void ShouldFailToAddANodeWithInvalidEdges()
    {
        var problem = new PathModel();
        var graph = Graph<string>.CreateReadOnly(problem);

        var exception1 = Assert.Throws<ArgumentException>(() =>
        {
            EdgeTuple<string>[] notExistingEdges = [ new() { NodeKey = "L", Cost = 10.0m } ];
            graph.Mod().AddNode("X", notExistingEdges, Option<EdgeTuple<string>[]>.None());
        });
        
        Assert.Equal("Edge to a node with a non existent key", exception1.Message);
    }
    
    [Fact]
    public void ShouldRemoveANodeToAGraph()
    {
        var problem = new PathModel();
        var graph = Graph<string>.CreateReadOnly(problem)
            .Mod().RemoveNode("Z").EndMod();

        Assert.IsAssignableFrom<IGraph<string>>(graph);
        Assert.False(graph.ContainsNode("Z"));
    }

    [Fact]
    public void ShouldCannotRemoveANotExistingNode()
    {
        var problem = new PathModel();
        var graph = Graph<string>.CreateReadOnly(problem);
        
        Assert.False(graph.ContainsNode("X"));
        
        var editedGraph = graph.Mod().RemoveNode("X").EndMod();

        Assert.NotSame(graph, editedGraph);
        Assert.False(editedGraph.ContainsNode("X"));
        Assert.Equal(graph.NodesCount, editedGraph.NodesCount);
    }

    [Fact]
    public void ShouldCreateAGraphFromAdjacencyMatrix()
    {
        var graph = Graph<int>.CreateReadOnly(new SccGraphModel1());
        
        Assert.Equal(10, graph.NodesCount);
        Assert.True(graph.AreConnected(0, 1));
        Assert.True(graph.AreConnected(2, 6));
        Assert.True(graph.AreConnected(9, 8));
    }
    
    [Fact]
    public void ShouldGraphBeUndirected()
    {
        var graph = Graph<int>.CreateReadOnly(new SparseUndirectedGraphModel1());
        
        Assert.True(graph.IsUndirected());
    }

    [Fact]
    public void ShouldFindACycle()
    {
        var graph = Graph<int>.CreateReadOnly(new SparseUndirectedGraphModel1());
        
        Assert.True(graph.IsCyclic());
    }
}