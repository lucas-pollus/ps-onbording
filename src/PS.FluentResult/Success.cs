namespace PS.FluentResult;

public partial struct Result
{
    /// <summary>
    ///     Cria um resultado de sucesso.
    /// </summary>
    public static Result Success()
    {
        return new Result(false, string.Empty);
    }

    /// <summary>
    ///     Cria um resultado de sucesso contendo o valor fornecido.
    /// </summary>
    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(false, string.Empty, value);
    }
}