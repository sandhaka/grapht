namespace GraphT.Graph.Architecture.Components;

internal record Edge<T>(Node<T> To, decimal Cost) where T : IEquatable<T>;