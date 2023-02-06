using PS.FluentResult.Exceptions;

namespace PS.FluentResult.Internal;

internal static class ResultCommonLogic
{
    internal static bool ErrorStateGuard(bool isFailure, string error)
    {
        if (isFailure)
        {
            if (string.IsNullOrEmpty(error))
            {
                throw new ArgumentNullException(nameof(error), "Quando um resultado é falha, o motivo do erro deve ser informado!");
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException("Quando um resultado é sucesso, não é permitido uma mensagem de erro!", nameof(error));
            }
        }
        return isFailure;
    }

    internal static string GetErrorWithSuccessGuard(bool isFailure, string error) =>
        isFailure ? error : throw new ResultSuccessException();
}