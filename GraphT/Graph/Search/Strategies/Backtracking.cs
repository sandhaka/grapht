using GraphT.Graph.Parameters;
using GraphT.Graph.Search.Context;
using Monads.Optional;

namespace GraphT.Graph.Search.Strategies;

public class Backtracking<TK> : IShortestPathSearchStrategy<TK> 
    where TK : IEquatable<TK>
{
    private decimal _bestCost = decimal.MaxValue;
    private IReadOnlyList<TK> _bestPath = [];
    
    public string Name => "Backtracking";

    public Option<Heuristic<TK>> Heuristic { get; set; } = Option<Heuristic<TK>>.None();
    
    public bool Run(IPathSearchContext<TK> context, out Option<SearchResult<TK>> result)
    {
        _bestCost = decimal.MaxValue;
        _bestPath = [];
        
        Backtrack(context, context.Start, [ context.Start ], 0);
        
        result = new SearchResult<TK>
        {
            Path = _bestPath,
            TotalCost = _bestCost
        };
        
        return _bestCost != decimal.MaxValue;
    }

    private void Backtrack(IPathSearchContext<TK> context, TK node, List<TK> path, decimal currentCost)
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
            var nodeKey = edge.NodeKey;
            var cost = edge.Cost;
            
            if (path.Contains(nodeKey))
                continue; // Avoiding loops

            var newCost = currentCost + cost;
            
            path.Add(nodeKey);

            Backtrack(context, nodeKey, path, newCost); // Recursion :(
            
            path.RemoveAt(path.Count - 1);
        }
    }
}