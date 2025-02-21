namespace GraphT.Graph.Constraints;

public delegate bool BinaryConstraint<T>(T from, T to, decimal cost = decimal.Zero) where T : IEquatable<T>;