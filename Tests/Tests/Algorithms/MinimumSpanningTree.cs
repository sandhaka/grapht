using GraphSamples;
using GraphT.Export;
using GraphT.Graph;
using Xunit.Abstractions;

namespace Tests.Tests.Algorithms;

public class MinimumSpanningTree(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);
    
    [Fact]
    public void ShouldFindTheMinimumSpanningTree()
    {
        var graph = Graph<int>.CreateReadOnly(new SparseUndirectedGraphModel1());
        
        var mst = graph.ReduceToMst();
        
        var nodesCount = mst.NodesCount;
        var edgesCount = mst.EdgesCount;
        
        var exported = mst.Export(GraphExportOutput.Gephi);
        
        _output.WriteLine(exported);

        // Verify the graph is still undirected
        Assert.True(mst.IsUndirected());
        // A Minimum Spanning Tree for a connected graph must have (nodesCount - 1) edges
        Assert.Equal(nodesCount - 1, edgesCount / 2);
        // Verify there are no cycles in the graph (an MST must be acyclic)
        Assert.False(mst.IsCyclic());
    }
}