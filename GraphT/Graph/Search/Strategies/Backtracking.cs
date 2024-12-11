using GraphT.Graph.Parameters;
using GraphT.Graph.Search.Context;
using Monads.Optional;

namespace GraphT.Graph.Search.Strategies;

public class Backtracking<T> : IPathSearchStrategy<T> 
    where T : IEquatable<T>
{
    private decimal _bestCost = decimal.MaxValue;
    private IReadOnlyList<T> _bestPath = [];
    
    public string Name => "Backtracking";

    public Option<Heuristic<T>> Heuristic { get; set; } = Option<Heuristic<T>>.None();
    
    public bool Run(IPathSearchContext<T> context, out Option<SearchResult<T>> result)
    {
        _bestCost = decimal.MaxValue;
        _bestPath = [];
        
        Backtrack(context, context.Start, [ context.Start ], 0);
        
        result = new SearchResult<T>
        {
            Path = _bestPath,
            TotalCost = _bestCost
        };
        
        return _bestCost != decimal.MaxValue;
    }

    private void Backtrack(IPathSearchContext<T> context, T node, List<T> path, decimal currentCost)
    {
        if (node.Equals(context.Target))
        {
            if (currentCost >= _bestCost) 
                return;
            
            _bestCost = currentCost;
            _bestPath = path.ToList().AsReadOnly();
            
            return;
        }

        foreach (var edge in context.NodeEdges(node))
        {
            var nodeValue = edge.NodeValue;
            var cost = edge.Cost;
            
            if (path.Contains(nodeValue))
                continue; // Avoiding loops

            var newCost = currentCost + cost;
            
            path.Add(nodeValue);

            Backtrack(context, nodeValue, path, newCost); // Recursion :(
            
            path.RemoveAt(path.Count - 1);
        }
    }
}