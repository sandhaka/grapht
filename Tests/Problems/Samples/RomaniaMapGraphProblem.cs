using System.Diagnostics;
using GraphT.Problems.Abstractions;

namespace Tests.Problems.Samples;

[DebuggerDisplay("{Name}")]
public record GeoNodeValue // Value geo-localized useful to test the heuristic
{
    public required string Name { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
}

public class RomaniaMapGraphProblem : IGraphProblem<GeoNodeValue>
{
    private readonly Dictionary<string, GeoNodeValue> _nodes = new()
    {
        ["Arad"] = new GeoNodeValue { Name = "Arad", X = 91, Y = 492 },
        ["Bucharest"] = new GeoNodeValue { Name = "Bucharest", X = 400, Y = 327 },
        ["Craiova"] = new GeoNodeValue { Name = "Craiova", X = 253, Y = 288 },
        ["Drobeta"] = new GeoNodeValue { Name = "Drobeta", X = 165, Y = 299 },
        ["Eforie"] = new GeoNodeValue { Name = "Eforie", X = 562, Y = 293 },
        ["Fagaras"] = new GeoNodeValue { Name = "Fagaras", X = 305, Y = 449 },
        ["Giurgiu"] = new GeoNodeValue { Name = "Giurgiu", X = 375, Y = 270 },
        ["Hirsova"] = new GeoNodeValue { Name = "Hirsova", X = 534, Y = 350 },
        ["Iasi"] = new GeoNodeValue { Name = "Iasi", X = 473, Y = 506 },
        ["Lugoj"] = new GeoNodeValue { Name = "Lugoj", X = 165, Y = 379 },
        ["Mehadia"] = new GeoNodeValue { Name = "Mehadia", X = 168, Y = 339 },
        ["Neamt"] = new GeoNodeValue { Name = "Neamt", X = 406, Y = 537 },
        ["Oradea"] = new GeoNodeValue { Name = "Oradea", X = 131, Y = 571 },
        ["Pitesti"] = new GeoNodeValue { Name = "Pitesti", X = 320, Y = 368 },
        ["Rimnicu"] = new GeoNodeValue { Name = "Rimnicu", X = 233, Y = 410 },
        ["Sibiu"] = new GeoNodeValue { Name = "Sibiu", X = 207, Y = 457 },
        ["Timisoara"] = new GeoNodeValue { Name = "Timisoara", X = 94, Y = 410 },
        ["Urziceni"] = new GeoNodeValue { Name = "Urziceni", X = 456, Y = 350 },
        ["Vaslui"] = new GeoNodeValue { Name = "Vaslui", X = 509, Y = 444 },
        ["Zerind"] = new GeoNodeValue { Name = "Zerind", X = 108, Y = 531 },
    };
    
    public GeoNodeValue Get(string key) => _nodes[key];

    public IDictionary<GeoNodeValue, List<(GeoNodeValue Value, decimal Cost)>> AdjacencyList
    {
        get // Generate a full graph map of Romanian cities,
            // sample taken from "Artificial Intelligence: A Modern approach" by Peter Norvig
        {
            var map = new Dictionary<GeoNodeValue, List<(GeoNodeValue Value, decimal Cost)>>
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
                    if(!map[ne.Value].Contains((n, ne.Cost)))
                        map[ne.Value].Add((n, ne.Cost));

            return map;
        }
    }
}