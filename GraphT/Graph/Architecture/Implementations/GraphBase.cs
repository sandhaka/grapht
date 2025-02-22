using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;
using GraphT.Graph.Constraints;
using GraphT.Graph.Dto;
using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal abstract class GraphBase<TK> : IGraph<TK>, IGraphComponents<TK> 
    where TK : IEquatable<TK>
{
    private readonly FrozenNodeCollection<TK> _nodesCollection;

    protected GraphBase(FrozenNodeCollection<TK> nodesCollection)
    {
        _nodesCollection = nodesCollection;
    }

    public IReadOnlySet<TK> NodeKeys => _nodesCollection.Keys;
    public Node<TK> this[TK key] => _nodesCollection[key];
    public Option<OnVisit<TK>> OnVisitActionParameter { get; set; } = Option<OnVisit<TK>>.None();
    public int NodesCount => _nodesCollection.NodesCount;
    public int EdgesCount => _nodesCollection.Sum(n => n.Edges.Length);

    public bool ContainsNode(TK key) => _nodesCollection.Contains(key);
    
    public bool AreConnected(TK start, TK end)
    {
        if (!_nodesCollection.Contains(start) || !_nodesCollection.Contains(end)) 
            return false;
        if (start.Equals(end)) 
            return true;
        
        var node = _nodesCollection[start];
        var visited = new HashSet<Node<TK>> { node };
        var queue = new Queue<Node<TK>>([node]);
        
        while (queue.Count > 0)
        {
            node = queue.Dequeue();
            
            if (node.Key.Equals(end)) 
                return true;

            var span = node.Edges.Span;
            for (var i = 0; i < node.Edges.Length; i++)
            {
                var child = span[i];
                if (!visited.Add(child.To)) 
                    continue;
                queue.Enqueue(child.To);
            }
        }

        return false;
    }

    public void TraverseDfs(TK start)
    {
        if (!_nodesCollection.Contains(start)) return;
        
        var node = _nodesCollection[start];
        var visited = new HashSet<Node<TK>>();
        var stack = new Stack<Node<TK>>([node]);

        while (stack.Count > 0)
        {
            node = stack.Pop();
            
            visited.Add(node);
            ActionExecute(node);
            
            var childrenLength = node.Edges.Length;
            
            for (var i = 0; i < childrenLength; i++)
            {
                var child = node.Edges.Span[i];
                if (visited.Contains(child.To)) 
                    continue;
                
                stack.Push(child.To);
            }
        }
    }
    
    public void TraverseBfs(TK start)
    {
        if (!_nodesCollection.Contains(start)) return;
        
        var node = _nodesCollection[start];
        var visited = new HashSet<Node<TK>>() { node };
        var queue = new Queue<Node<TK>>([node]);
        
        while (queue.Count > 0)
        {
            node = queue.Dequeue();
            
            ActionExecute(node);
            
            var childrenLength = node.Edges.Length;
            
            for (var i = 0; i < childrenLength; i++)
            {
                var child = node.Edges.Span[i];
                if (!visited.Add(child.To)) 
                    continue;
                
                queue.Enqueue(child.To);
            }
        }
    }
    
    public IGraphMod<TK> Mod() => MutableGraph<TK>.Create(this, _nodesCollection.ToArray());

    public IPathSearch<TK> ToPathSearch() => new PathSearch<TK>(this);
    
    public IPathSearch<TK> ToPathSearch(IShortestPathSearchStrategy<TK> shortestPathSearchStrategy) => 
        new PathSearch<TK>(this, shortestPathSearchStrategy);
    
    public IGraph<TK> ReduceToMst()
    {
        if (!IsUndirected()) throw new InvalidOperationException("Graph is not undirected");
        var mstNodes = new Prims<TK>(_nodesCollection).Mst();
        return RebuildGraph(mstNodes);
    }

    private IGraph<TK> RebuildGraph(Option<HashSet<Node<TK>>> nodes) =>
        nodes.IsNone ? this : ReadOnlyGraph<TK>.Create(nodes.Reduce([]));

    public bool IsUndirected()
    {
        foreach (var node in _nodesCollection)
        {
            if (node.Edges.Length == 0) 
                return false;
            
            // Check if the reverse edge (child -> node) exists
            var span = node.Edges.Span;
            for (var i = 0; i < node.Edges.Length; i++)
            {
                var reverseEdgeExists = false;
                
                var edge = span[i];
                var childSpan = edge.To.Edges.Span;
                for (var j = 0; j < edge.To.Edges.Length; j++)
                {
                    if (childSpan[j].To.Key.Equals(node.Key))
                        reverseEdgeExists = true;
                }
                
                if (!reverseEdgeExists)
                    return false;
            }
        }
        
        return true; // All edges are reciprocal, the graph is undirected
    }

    public IReadOnlyList<EdgeData<TK>> AllEdges()
    {
        var allEdges = new List<EdgeData<TK>>();
        
        foreach (var node in _nodesCollection)
        {
            var edges = node.Edges.Span;
            for (var i = 0; i < node.Edges.Length; i++)
            {
                var cost = edges[i].Cost;
                var to = edges[i].To.Key;
                var from = edges[i].From.Key;
                
                allEdges.Add(new EdgeData<TK>(from, cost, to));
            }
        }
        
        return allEdges.AsReadOnly();
    }

    public ICsp<TK> ToCsp(IDictionary<TK, Domain> domains, IDictionary<TK, IConstraint[]> constraints)
    {
        throw new NotImplementedException();
    }

    public bool IsCyclic() => FindCycle();

    private bool FindCycle(Node<TK>? node = null, HashSet<TK>? visited = null, Node<TK>? parent = null)
    {
        node ??= _nodesCollection.RandomNode();
        var visitedSet = visited ?? new HashSet<TK>();

        if (!visitedSet.Add(node.Key)) // Mark node as visited
            return false;

        var span = node.Edges.Span;
        // Traverse through all adjacent nodes
        for (var i = 0; i < node.Edges.Length; i++)
        {
            var child = span[i];
            // If the adjacent node is not visited, recurse
            if (!visitedSet.Contains(child.To.Key))
            {
                if (FindCycle(child.To, visitedSet, node)) // Refactoring: leading to stack overflow
                    return true;
            }
            
            // If the adjacent node is visited and is not the parent, a cycle is found
            else if (child.To != parent)
            {
                return true;
            }
        }

        return false;
    }

    protected virtual void ActionExecute(Node<TK> node)
    {
        var action = OnVisitActionParameter.Reduce(_ => { });
        
        action(node.Key);
    }
}