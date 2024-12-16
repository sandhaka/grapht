using GraphT.Graph.Dto;
using Monads.Optional;

namespace GraphT.Graph.Architecture.NodeCollections.Abstractions;

internal interface IMutableNodeCollection<T> : INodeCollection<T>
    where T : IEquatable<T>
{
    void Add(T value, Option<EdgeTuple<T>[]> inEdges, Option<EdgeTuple<T>[]> outEdges);
    void Remove(T value);
}