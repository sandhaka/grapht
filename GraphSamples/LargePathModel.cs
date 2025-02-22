using GraphT.Models.Abstractions;

namespace GraphSamples;

public class LargeGraphListModel : IGraphListModel<string>
{
    public IDictionary<string, List<(string Key, decimal Cost)>> AdjacencyList =>
        new Dictionary<string, List<(string Key, decimal Cost)>>
            {
                { "Node1", new List<(string, decimal)> { ("Node2", 1), ("Node3", 4) } },
                { "Node2", new List<(string, decimal)> { ("Node1", 1), ("Node4", 2), ("Node5", 3) } },
                { "Node3", new List<(string, decimal)> { ("Node1", 4), ("Node6", 5) } },
                { "Node4", new List<(string, decimal)> { ("Node2", 2), ("Node7", 1) } },
                { "Node5", new List<(string, decimal)> { ("Node2", 3), ("Node8", 7), ("Node9", 4) } },
                { "Node6", new List<(string, decimal)> { ("Node3", 5), ("Node10", 8) } },
                { "Node7", new List<(string, decimal)> { ("Node4", 1), ("Node11", 2) } },
                { "Node8", new List<(string, decimal)> { ("Node5", 7), ("Node12", 4) } },
                { "Node9", new List<(string, decimal)> { ("Node5", 4), ("Node13", 5) } },
                { "Node10", new List<(string, decimal)> { ("Node6", 8), ("Node14", 3) } },
                { "Node11", new List<(string, decimal)> { ("Node7", 2), ("Node15", 6) } },
                // Continuing pattern to Node100
            }
            .MergeWith(GenerateGraphPart(12, 100, 2, 10)); // Filling up to 100 nodes

    private IDictionary<string, List<(string Key, decimal Cost)>> GenerateGraphPart(
        int startNode, int endNode, int minCost, int maxCost)
    {
        var random = new Random();
        var adjacencyList = new Dictionary<string, List<(string Key, decimal Cost)>>();

        for (int i = startNode; i <= endNode; i++)
        {
            var connections = new List<(string Key, decimal Cost)>();
            if (i > startNode)
                connections.Add(($"Node{i - 1}", random.Next(minCost, maxCost))); // Link to previous node
            if (i < endNode)
                connections.Add(($"Node{i + 1}", random.Next(minCost, maxCost))); // Link to next node
            adjacencyList[$"Node{i}"] = connections;
        }

        return adjacencyList;
    }
}

internal static class AdjacencyListExtensions
{
    public static IDictionary<string, List<(string Key, decimal Cost)>> MergeWith(
        this IDictionary<string, List<(string Key, decimal Cost)>> original,
        IDictionary<string, List<(string Key, decimal Cost)>> other)
    {
        foreach (var node in other)
        {
            if (!original.ContainsKey(node.Key))
            {
                original[node.Key] = node.Value;
            }
            else
            {
                original[node.Key].AddRange(node.Value);
            }
        }

        return original;
    }
}