using PS.FluentResult.Internal;

namespace PS.FluentResult;

public partial struct Result
{
    public bool IsFailure { get; }

    public bool IsSuccess => !IsFailure;

    private readonly string error;
    public string Error => ResultCommonLogic.GetErrorWithSuccessGuard(IsFailure, error);

    private Result(bool isFailure, string error)
    {
        IsFailure = ResultCommonLogic.ErrorStateGuard(isFailure, error);
        this.error = error;
    }

    public void EnsureSuccess()
    {
        if (IsFailure)
        {
            throw new Exception(error);
        }
    }
}