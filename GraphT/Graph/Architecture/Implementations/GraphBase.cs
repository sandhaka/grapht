using GraphT.Graph.Architecture.Abstractions;
using GraphT.Graph.Architecture.Algorithms;
using GraphT.Graph.Architecture.Components;
using GraphT.Graph.Architecture.NodeCollections;
using GraphT.Graph.Dto;
using GraphT.Graph.Parameters;
using GraphT.Graph.Search;
using Monads.Optional;

namespace GraphT.Graph.Architecture.Implementations;

internal abstract class GraphBase<T> : IGraph<T>, IGraphComponents<T> 
    where T : IEquatable<T>
{
    private readonly FrozenNodeCollection<T> _nodesCollection;

    protected GraphBase(FrozenNodeCollection<T> nodesCollection)
    {
        _nodesCollection = nodesCollection;
    }

    public IReadOnlySet<T> NodeValues => _nodesCollection.Values;
    public Node<T> this[T value] => _nodesCollection[value];
    public Option<OnVisit<T>> OnVisitActionParameter { get; set; } = Option<OnVisit<T>>.None();
    public int NodesCount => _nodesCollection.NodesCount;
    public int EdgesCount => _nodesCollection.Sum(n => n.Edges.Length);

    public bool ContainsNode(T value) => _nodesCollection.Contains(value);
    
    public bool AreConnected(T start, T end)
    {
        if (!_nodesCollection.Contains(start) || !_nodesCollection.Contains(end)) 
            return false;
        if (start.Equals(end)) 
            return true;
        
        var node = _nodesCollection[start];
        var visited = new HashSet<Node<T>> { node };
        var queue = new Queue<Node<T>>([node]);
        
        while (queue.Count > 0)
        {
            node = queue.Dequeue();
            
            if (node.Value.Equals(end)) 
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

    public void TraverseDfs(T start)
    {
        if (!_nodesCollection.Contains(start)) return;
        
        var node = _nodesCollection[start];
        var visited = new HashSet<Node<T>>();
        var stack = new Stack<Node<T>>([node]);

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
    
    public void TraverseBfs(T start)
    {
        if (!_nodesCollection.Contains(start)) return;
        
        var node = _nodesCollection[start];
        var visited = new HashSet<Node<T>>() { node };
        var queue = new Queue<Node<T>>([node]);
        
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
    
    public IGraphMod<T> Mod() => MutableGraph<T>.Create(this, _nodesCollection.ToArray());

    public IPathSearch<T> ToPathSearch() => new PathSearch<T>(this);
    
    public IPathSearch<T> ToPathSearch(IShortestPathSearchStrategy<T> shortestPathSearchStrategy) => 
        new PathSearch<T>(this, shortestPathSearchStrategy);
    
    public IGraph<T> ReduceToMst()
    {
        if (!IsUndirected()) throw new InvalidOperationException("Graph is not undirected");
        var mstNodes = new Prims<T>(_nodesCollection).Mst();
        return RebuildGraph(mstNodes);
    }

    private IGraph<T> RebuildGraph(Option<HashSet<Node<T>>> nodes) =>
        nodes.IsNone ? this : ReadOnlyGraph<T>.Create(nodes.Reduce([]));

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
                    if (childSpan[j].To.Value.Equals(node.Value))
                        reverseEdgeExists = true;
                }
                
                if (!reverseEdgeExists)
                    return false;
            }
        }
        
        return true; // All edges are reciprocal, the graph is undirected
    }

    public IReadOnlyList<EdgeData<T>> AllEdges()
    {
        var allEdges = new List<EdgeData<T>>();
        
        foreach (var node in _nodesCollection)
        {
            var edges = node.Edges.Span;
            for (var i = 0; i < node.Edges.Length; i++)
            {
                var cost = edges[i].Cost;
                var to = edges[i].To.Value;
                
                allEdges.Add(new EdgeData<T>(node.Value, cost, to));
            }
        }
        
        return allEdges.AsReadOnly();
    }
    
    public bool IsCyclic() => FindCycle();

    private bool FindCycle(Node<T>? node = null, HashSet<T>? visited = null, Node<T>? parent = null)
    {
        node ??= _nodesCollection.RandomNode();
        var visitedSet = visited ?? new HashSet<T>();

        if (!visitedSet.Add(node.Value)) // Mark node as visited
            return false;

        var span = node.Edges.Span;
        // Traverse through all adjacent nodes
        for (var i = 0; i < node.Edges.Length; i++)
        {
            var child = span[i];
            // If the adjacent node is not visited, recurse
            if (!visitedSet.Contains(child.To.Value))
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

    protected virtual void ActionExecute(Node<T> node)
    {
        var action = OnVisitActionParameter.Reduce(_ => { });
        
        action(node.Value);
    }
}