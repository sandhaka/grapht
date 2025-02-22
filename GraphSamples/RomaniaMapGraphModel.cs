using System.Diagnostics;
using GraphT.Models.Abstractions;

namespace GraphSamples;

[DebuggerDisplay("{Name}")]
public record GeoNode // Value geo-localized useful to test the heuristic
{
    public required string Name { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
}

public class RomaniaMapGraphModel : IGraphListModel<GeoNode>
{
    private readonly Dictionary<string, GeoNode> _nodes = new()
    {
        ["Arad"] = new GeoNode { Name = "Arad", X = 91, Y = 492 },
        ["Bucharest"] = new GeoNode { Name = "Bucharest", X = 400, Y = 327 },
        ["Craiova"] = new GeoNode { Name = "Craiova", X = 253, Y = 288 },
        ["Drobeta"] = new GeoNode { Name = "Drobeta", X = 165, Y = 299 },
        ["Eforie"] = new GeoNode { Name = "Eforie", X = 562, Y = 293 },
        ["Fagaras"] = new GeoNode { Name = "Fagaras", X = 305, Y = 449 },
        ["Giurgiu"] = new GeoNode { Name = "Giurgiu", X = 375, Y = 270 },
        ["Hirsova"] = new GeoNode { Name = "Hirsova", X = 534, Y = 350 },
        ["Iasi"] = new GeoNode { Name = "Iasi", X = 473, Y = 506 },
        ["Lugoj"] = new GeoNode { Name = "Lugoj", X = 165, Y = 379 },
        ["Mehadia"] = new GeoNode { Name = "Mehadia", X = 168, Y = 339 },
        ["Neamt"] = new GeoNode { Name = "Neamt", X = 406, Y = 537 },
        ["Oradea"] = new GeoNode { Name = "Oradea", X = 131, Y = 571 },
        ["Pitesti"] = new GeoNode { Name = "Pitesti", X = 320, Y = 368 },
        ["Rimnicu"] = new GeoNode { Name = "Rimnicu", X = 233, Y = 410 },
        ["Sibiu"] = new GeoNode { Name = "Sibiu", X = 207, Y = 457 },
        ["Timisoara"] = new GeoNode { Name = "Timisoara", X = 94, Y = 410 },
        ["Urziceni"] = new GeoNode { Name = "Urziceni", X = 456, Y = 350 },
        ["Vaslui"] = new GeoNode { Name = "Vaslui", X = 509, Y = 444 },
        ["Zerind"] = new GeoNode { Name = "Zerind", X = 108, Y = 531 },
    };
    
    public GeoNode Get(string key) => _nodes[key];

    public IDictionary<GeoNode, List<(GeoNode Key, decimal Cost)>> AdjacencyList
    {
        get // Generate a full graph map of Romanian cities,
            // sample taken from "Artificial Intelligence: A Modern approach" by Peter Norvig
        {
            var map = new Dictionary<GeoNode, List<(GeoNode Key, decimal Cost)>>
            {
                [_nodes["Arad"]] = [ (_nodes["Zerind"], 75m), (_nodes["Sibiu"], 140m), (_nodes["Timisoara"], 118m) ],
                [_nodes["Bucharest"]] = [ (_nodes["Urziceni"], 85m), (_nodes["Pitesti"], 101m),(_nodes["Giurgiu"], 90m), (_nodes["Fagaras"], 211m)  ],
                [_nodes["Craiova"]] = [ (_nodes["Drobeta"], 120m), (_nodes["Rimnicu"], 146m), (_nodes["Pitesti"], 138m) ],
                [_nodes["Drobeta"]] = [ (_nodes["Mehadia"], 75m) ],
                [_nodes["Eforie"]] = [ (_nodes["Hirsova"], 86m) ],
                [_nodes["Fagaras"]] = [ (_nodes["Sibiu"], 99m) ],
                [_nodes["Hirsova"]] = [ (_nodes["Urziceni"], 98m) ],
                [_nodes["Iasi"]] = [ (_nodes["Vaslui"], 92m), (_nodes["Neamt"], 87m) ],
                [_nodes["Lugoj"]] = [ (_nodes["Timisoara"], 111m), (_nodes["Mehadia"], 70m) ],
                [_nodes["Oradea"]] = [ (_nodes["Zerind"], 71m), (_nodes["Sibiu"], 151m) ],
                [_nodes["Pitesti"]] = [ (_nodes["Rimnicu"], 97m) ],
                [_nodes["Rimnicu"]] = [ (_nodes["Sibiu"], 80m) ],
                [_nodes["Urziceni"]] = [ (_nodes["Vaslui"], 142m) ],
                [_nodes["Zerind"]] = [],
                [_nodes["Giurgiu"]] = [],
                [_nodes["Mehadia"]] = [],
                [_nodes["Neamt"]] = [],
                [_nodes["Sibiu"]] = [],
                [_nodes["Timisoara"]] = [],
                [_nodes["Vaslui"]] = []
            };

            // Build the undirected graph
            foreach (var (n, values) in map)
                foreach (var ne in values)
                    if(!map[ne.Key].Contains((n, ne.Cost)))
                        map[ne.Key].Add((n, ne.Cost));

            return map;
        }
    }
}