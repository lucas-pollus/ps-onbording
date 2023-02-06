using System.Data;
using PS.Data.Models;
using PS.FluentResult;

namespace PS.Data;

public interface IDbHelper
{
    Task<Result> ExecuteAsync(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null
    );

    Task<Result<T>> GetAsync<T>(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null
    );

    Task<Result<T>> CreateAsync<T>(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null
    );

    Task<Result<QueryOutput<T>>> PagedList<T>(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null
    );
}