namespace GraphT.Graph.Dto;

public record EdgeData<T>(T From, decimal Cost, T To) where T : IEquatable<T>
{
    public void Deconstruct(out T from, out decimal cost, out T to)
    {
        from = From;
        cost = Cost;
        to = To;
    }
}