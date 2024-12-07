using GraphT.Graph.Search;
using Monads.Optional;

namespace Tests;

public static class SearchResultStringExtensions
{
    public static string GetPath(this Option<SearchResult<string>> result, out decimal cost)
    {
        var reducedResult = result.Reduce(new SearchResult<string> { Path = new List<string>(), TotalCost = 0 });
        var resultPath = string.Join(',', reducedResult.Path);
        cost = reducedResult.TotalCost;
        return resultPath;
    }
}