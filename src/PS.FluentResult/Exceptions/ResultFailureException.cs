namespace PS.FluentResult.Exceptions;

public class ResultFailureException : Exception
{
    public string Error { get; }

    internal ResultFailureException(string error)
        : base($"Você não pode obter um valor do resultado quando ele for falha. Erro: {error}") => Error = error;
}