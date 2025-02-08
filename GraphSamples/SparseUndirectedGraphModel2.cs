using GraphT.Models.Abstractions;

namespace GraphSamples;

public class SparseUndirectedGraphModel2 : IGraphMatrixModel<int>
{
    public decimal[,] Matrix => new decimal[,]
    {
        { 0, 4, 5, 6, 5, 10, 6, 4, 0, 0 },
        { 4, 0, 9, 10, 1, 0, 0, 0, 0, 0 },
        { 5, 9, 0, 1, 7, 0, 0, 7, 6, 5 },
        { 6, 10, 1, 0, 3, 7, 0, 0, 2, 0 },
        { 5, 1, 7, 3, 0, 0, 0, 0, 0, 8 },
        { 10, 0, 0, 7, 0, 0, 0, 5, 0, 0 },
        { 6, 0, 0, 0, 0, 0, 0, 9, 8, 0 },
        { 4, 0, 7, 0, 0, 5, 9, 0, 9, 0 },
        { 0, 0, 6, 2, 0, 0, 8, 9, 0, 8 },
        { 0, 0, 5, 0, 8, 0, 0, 0, 8, 0 },
    };

    public int[] Nodes => Enumerable.Range(0, 10).ToArray();
}