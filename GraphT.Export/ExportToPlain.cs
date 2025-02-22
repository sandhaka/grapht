using GraphT.Graph;

namespace GraphT.Export;

internal class ExportToPlain<TK>(IGraph<TK> graph) where TK : IEquatable<TK>
{
    public string Export()
    {
        throw new NotImplementedException();
    }
}