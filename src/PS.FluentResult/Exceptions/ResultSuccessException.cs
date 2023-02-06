namespace PS.FluentResult.Exceptions;

public class ResultSuccessException : Exception
{
    internal ResultSuccessException()
        : base("Você não pode obter o erro quando resultado for um sucesso!")
    {
    }
}