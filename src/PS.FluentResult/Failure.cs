namespace PS.FluentResult;

public partial struct Result
{
    /// <summary>
    ///     Cria um resultado de falha com a mensagem de erro fornecida.
    /// </summary>
    public static Result Failure(string error)
    {
        return new Result(true, error);
    }

    /// <summary>
    ///     Cria um resultado de falha com a mensagem de erro fornecida.
    /// </summary>
    public static Result<T> Failure<T>(string error)
    {
        return new Result<T>(true, error, default!);
    }
}