using GraphSamples;
using GraphT.Graph;
using Xunit.Abstractions;

namespace Tests.Tests.Algorithms;

public class MinimumSpanningTree(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);
    
    [Fact]
    public void ShouldFindTheMinimumSpanningTree()
    {
        var graph = Graph<int>.CreateReadOnly(new SccGraphModel2());

        var minimumSpanningTree = graph.Mst().ToHashSet();
        
        Assert.Equal(graph.NodesCount - 1, minimumSpanningTree.Count());
    }
    
    [Fact]
    public void ShouldFindTheMinimumSpanningTreeOfaSparseGraph1()
    {
        var graph = Graph<int>.CreateReadOnly(new SparseGraphModel1());

        var minimumSpanningTree = graph.Mst().ToHashSet();
        
        Assert.Equal(graph.NodesCount - 1, minimumSpanningTree.Count());
    }
}