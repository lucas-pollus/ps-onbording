using PS.Data;
using PS.Data.Models;

namespace PS.Web.Models;

public class QueryResponse<TResponse>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalRows { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public IEnumerable<TResponse> Values { get; set; }
}

public static class DynamicEnumerable
{
    public static IEnumerable<TDestination> ToEnumerableResponse<TSource, TDestination>(this IEnumerable<TSource> source)
    {
        foreach (dynamic current in source)
        {
            yield return (TDestination)current;
        }
    }
}

public static class QueryResponseExtensions
{
    public static QueryResponse<TResponse> ToResponse<TResult, TResponse>(this QueryOutput<TResult> result)
        where TResult : IQueryOutput
    {
        return new()
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages,
            TotalRows = result.TotalPages,
            HasNextPage = result.HasNextPage,
            Values = result.Values.ToEnumerableResponse<TResult, TResponse>()
        };
    }
}
