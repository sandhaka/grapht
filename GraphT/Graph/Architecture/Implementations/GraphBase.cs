using GraphT.Graph.Architecture.Abstractions;
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

    public IGraphMod<T> Mod() => MutableGraph<T>.Create(this, _nodesCollection.ToArray());

    public IPathSearch<T> ToPathSearch() => new PathSearch<T>(this);
    
    public IPathSearch<T> ToPathSearch(IShortestPathSearchStrategy<T> shortestPathSearchStrategy) => 
        new PathSearch<T>(this, shortestPathSearchStrategy);

    public IEnumerable<EdgeData<T>> Mst()
    {
        var prims = new Prims<T>(_nodesCollection);
        return prims.FindMst();
    }

    public int NodesCount => _nodesCollection.NodesCount;

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

    protected virtual void ActionExecute(Node<T> node)
    {
        var action = OnVisitActionParameter.Reduce(_ => { });
        
        action(node.Value);
    }
}