namespace PS.Web.Models;

public class PagedListRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }

    public PagedListRequest()
    {
        Page = 1;
        PageSize = 10;
    }
}