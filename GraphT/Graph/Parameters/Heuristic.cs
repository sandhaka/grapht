using GraphT.Graph.Search.Context;

namespace GraphT.Graph.Parameters;

public delegate decimal Heuristic<TK>(TK node, IPathSearchContext<TK> context) 
    where TK : IEquatable<TK>;
