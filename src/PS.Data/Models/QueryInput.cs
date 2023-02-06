namespace PS.Data.Models;

public class QueryInput
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public QueryInput()
    {
        Page = 1;
        PageSize = 10;
    }
}
