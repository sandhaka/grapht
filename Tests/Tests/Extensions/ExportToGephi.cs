using GraphSamples;
using GraphT.Export;
using GraphT.Graph;
using Xunit.Abstractions;

namespace Tests.Tests.Extensions;

public class ExportToGephi(ITestOutputHelper output)
{
    private readonly VerifiableTestOutputHelper _output = new(output);

    [Fact]
    public void ShouldExportToGephi()
    {
        var graph = Graph<int>.CreateReadOnly(new SparseUndirectedGraphModel1());

        var exported = graph.Export(GraphExportOutput.Gephi);
        
        _output.WriteLine(exported);
    }
}