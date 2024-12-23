using GraphT.Graph.Search.Context;

namespace GraphT.Graph.Parameters;

public delegate decimal Heuristic<T>(T node, IPathSearchContext<T> context) 
    where T : IEquatable<T>;
