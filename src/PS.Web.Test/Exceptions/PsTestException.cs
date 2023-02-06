namespace PS.Web.Test.Exceptions;

internal class PsTestException : Exception
{
    internal PsTestException(string message)
        : base(message)
    {
    }
}