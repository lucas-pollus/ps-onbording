using PS.Data.Models;

namespace PS.Web.Models;

public class QueryRequest : PagedListRequest
{
    public static implicit operator QueryInput(QueryRequest req)
    {
        return new()
        {
            Page = req.Page,
            PageSize = req.PageSize
        };
    }
}