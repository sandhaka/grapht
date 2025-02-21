using GraphT.Graph.Constraints;

namespace GraphT.Graph.Architecture.Components;

internal record Edge<T>(Node<T> From, Node<T> To, decimal Cost) where T : IEquatable<T>;