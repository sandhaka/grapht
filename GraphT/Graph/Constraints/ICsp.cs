namespace GraphT.Graph.Constraints;

public interface ICsp<TK>
   where TK : IEquatable<TK>
{
    void Propagate();
}