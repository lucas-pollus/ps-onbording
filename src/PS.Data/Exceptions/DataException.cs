namespace PS.Data.Exceptions;
public class DataException : Exception
{
    public string Error { get; }

    internal DataException(string error)
        : base($"Falha ao processar requisição. Erro: {error}") => Error = error;
}