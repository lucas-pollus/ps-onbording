using PS.FluentResult.Exceptions;
using PS.FluentResult.Internal;

namespace PS.FluentResult;

public partial struct Result<T>
{
    private readonly T value;
    public T Value => IsSuccess ? value : throw new ResultFailureException(error);

    public bool IsFailure { get; }

    public bool IsSuccess => !IsFailure;

    private readonly string error;
    public string Error => ResultCommonLogic.GetErrorWithSuccessGuard(IsFailure, error);

    internal Result(bool isFailure, string error, T value)
    {
        IsFailure = ResultCommonLogic.ErrorStateGuard(isFailure, error);
        this.error = error;
        this.value = value;
    }

    public Result<T> EnsureSuccess()
    {
        if (IsFailure)
        {
            throw new Exception(error);
        }
        return this;
    }
}