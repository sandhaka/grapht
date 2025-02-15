using GraphSamples;
using GraphT.Export;
using GraphT.Graph;
using Xunit.Abstractions;

namespace Tests.Tests.Algorithms;

public class MinimumSpanningTree(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);

    [Fact]
    public void ShouldRiseAnExceptionIfGraphIsNotUndirected()
    {
        var graph = Graph<string>.CreateReadOnly(new SimpleGraphModel());
        
        Assert.Throws<InvalidOperationException>(graph.ReduceToMst);
    }
    
    [Fact]
    public void ShouldFindTheMinimumSpanningTree1()
    {
        var graph = Graph<int>.CreateReadOnly(new SparseUndirectedGraphModel1());
        
        var mst = graph.ReduceToMst();
        
        var nodesCount = mst.NodesCount;
        var edgesCount = mst.EdgesCount;

        // Verify the graph is still undirected
        Assert.True(mst.IsUndirected());
        // A Minimum Spanning Tree for a connected graph must have (nodesCount - 1) edges
        Assert.Equal(nodesCount - 1, edgesCount / 2);
        // Must be acyclic
        Assert.False(mst.IsCyclic());
    }

    [Fact]
    public void ShouldFindTheMinimumSpanningTree2()
    {
        var graph = Graph<int>.CreateReadOnly(new SparseUndirectedGraphModel2());
        var mst = graph.ReduceToMst();
        
        var nodesCount = mst.NodesCount;
        var edgesCount = mst.EdgesCount;
        
        // Verify the graph is still undirected
        Assert.True(mst.IsUndirected());
        // A Minimum Spanning Tree for a connected graph must have (nodesCount - 1) edges
        Assert.Equal(nodesCount - 1, edgesCount / 2);
        // Must be acyclic
        Assert.False(mst.IsCyclic());
    }
}