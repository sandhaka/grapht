namespace GraphT.Graph.Architecture.Components;

internal record Edge<TK>(Node<TK> From, Node<TK> To, decimal Cost) where TK : IEquatable<TK>;