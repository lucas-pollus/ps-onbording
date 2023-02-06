namespace PS.Data.Models;

public class QueryOutput<T>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalRows { get; init; }
    public int TotalPages { get; init; }
    public bool HasNextPage { get; init; }
    public IEnumerable<T> Values { get; set; }
    public QueryOutput(IEnumerable<T> values) => Values = values;
    public QueryOutput() { }
    internal void SetValues(IEnumerable<T> values)
    {
        Values = values;
    }
}