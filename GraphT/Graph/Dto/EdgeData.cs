namespace GraphT.Graph.Dto;

public record EdgeData<T>(T From, decimal Cost, T To) where T : IEquatable<T>;