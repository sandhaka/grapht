using GraphT.Models.Abstractions;

namespace GraphSamples;

public class SccGraphModel1 : IGraphMatrixModel<int>
{
    public decimal[,] Matrix => new decimal[,]
    {
        { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
        { 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
        { 1, 1, 0, 0, 0, 0, 1, 0, 1, 1 },
        { 1, 0, 0, 0, 1, 1, 1, 1, 0, 0 },
        { 0, 0, 0, 1, 0, 1, 1, 0, 0, 0 },
        { 0, 0, 0, 1, 1, 0, 1, 0, 0, 0 },
        { 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
        { 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 },
        { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 }
    };

    public int[] Nodes => [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
}