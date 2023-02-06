using FluentValidation.Results;

namespace PS.Web.Services;

public static partial class ServicesExtensions
{
    public static string Reduce(this IList<ValidationFailure> failures)
    {
        return string.Join(" ", failures.Select(failure => failure.ErrorMessage));
    }
}