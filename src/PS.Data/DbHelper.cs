using Dapper;
using System.Data;
using PS.FluentResult;
using Oracle.ManagedDataAccess.Client;
using PS.Data.Extensions;
using Dapper.Oracle;
using PS.Data.Models;

namespace PS.Data;

public class DbHelper : IDbHelper, IDisposable
{
    public IDbConnection Connection { get; }
    private const int PS_APPLICATION_ERROR = 20999;
    private const int ORA_INVALID_DATATYPE = 902;

    public DbHelper(IDbConnection connection, ConnectionString connString)
    {
        connection.ConnectionString = connString.Value;
        connection.Open();
        Connection = connection;
    }

    public Task<Result> ExecuteAsync(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null)
    {
        try
        {
            Connection.ExecuteAsync(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType: CommandType.StoredProcedure).GetAwaiter().GetResult();
        }
        catch (OracleException ex)
            when (ex.Number == PS_APPLICATION_ERROR)
        {
            return Task.FromResult(Result.Failure(ex.Message.ExtraiMensagem()));
        }
        return Task.FromResult(Result.Success());
    }

    public async Task<Result<T>> GetAsync<T>(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null)
    {
        var oraParam = new OracleDynamicParameters(param);
        oraParam.Add("po_result", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.ReturnValue);
        try
        {
            using var multi = await Connection.QueryMultipleAsync(
                sql,
                oraParam,
                transaction,
                commandTimeout, CommandType.StoredProcedure);

            var result = await multi.ReadSingleOrDefaultAsync<T>();

            return Result.Success(result);
        }
        catch (OracleException ex)
            when (ex.Number == PS_APPLICATION_ERROR)
        {
            return Result.Failure<T>(ex.Message.ExtraiMensagem());
        }
    }

    public Task<Result<T>> CreateAsync<T>(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null)
    {
        return GetAsync<T>(sql, param, transaction, commandTimeout);
    }

    public async Task<Result<QueryOutput<T>>> PagedList<T>(
        string sql,
        object param = null,
        IDbTransaction transaction = null,
        int? commandTimeout = null)
    {
        var oraParam = new OracleDynamicParameters(param);
        if (!oraParam.ParameterNames.Contains("po_result"))
        {
            oraParam.Add("po_result", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        }

        if (!oraParam.ParameterNames.Contains("po_info_query"))
        {
            oraParam.Add("po_info_query", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        }

        try
        {
            using var multi = await Connection.QueryMultipleAsync(
                    sql,
                    param: oraParam,
                    transaction: transaction,
                    commandTimeout: commandTimeout,
                    commandType: CommandType.StoredProcedure
                );

            var values = await multi.ReadAsync<T>();
            var queryResult = await multi.ReadSingleOrDefaultAsync<QueryOutput<T>>();
            queryResult?.SetValues(values);
            return Result.Success(queryResult);
        }
        catch (OracleException ex)
            when (ex.Number == PS_APPLICATION_ERROR)
        {
            return Result.Failure<QueryOutput<T>>(ex.Message.ExtraiMensagem());

        }
    }

    public void Dispose()
    {
        if (Connection.State != ConnectionState.Closed)
        {
            Connection.Close();
        }
        GC.SuppressFinalize(this);
    }
}