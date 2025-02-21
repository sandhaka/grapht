namespace GraphT.Graph.Constraints;

public delegate bool UnaryConstraint<T>(T v) where T : IEquatable<T>;    