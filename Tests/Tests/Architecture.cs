using GraphT.Graph;
using GraphT.Graph.Exceptions;
using GraphT.Graph.Parameters;
using ProblemSamples;
using Xunit.Abstractions;

namespace Tests.Tests;

public class Architecture(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);

    [Fact]
    public void ShouldBuildReadOnlyGraph()
    {
        var problem = new SimpleGraphProblem();

        var graph = Graph<string>.CreateReadOnly(problem);

        Assert.Equal(problem.AdjacencyList.Keys.Count, graph.NodeValues.Count);
    }

    [Fact]
    public void ShouldFailBuildReadOnlyGraph()
    {
        var problem = new SimpleBuggedGraphProblem();

        Assert.Throws<InvalidGraphDataException<string>>(() => { Graph<string>.CreateReadOnly(problem); });
    }

    [Fact]
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

    [Fact]
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
}