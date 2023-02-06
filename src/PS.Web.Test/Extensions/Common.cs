using Flurl.Http;
using PS.Data.Models;
using PS.Web.Models;
using PS.Web.Test.Exceptions;

namespace PS.Web.Test.Extensions;

public static class CommonExtensions
{
    public static QueryOutput<T> EnsureValuesContainsRows<T>(this QueryOutput<T> result)
    {
        if (!result.Values.Any())
        {
            throw new PsTestException("O resultado não possui nenhum registro.");
        }
        return result;
    }

    public static QueryResponse<T> EnsureValuesContainsRows<T>(this QueryResponse<T> queryResponse)
    {
        if (!queryResponse.Values.Any())
        {
            throw new PsTestException("O resultado não possui nenhum registro.");
        }
        return queryResponse;
    }

    public static Task<ErrorOutput> GetErrorAsync(this IFlurlResponse response)
    {
        return response.GetJsonAsync<ErrorOutput>();
    }
}