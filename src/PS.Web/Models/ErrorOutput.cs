namespace PS.Web.Models;

public class ErrorOutput
{
    public string Code { get; }
    public string Message { get; }
    public string DetailedMessage { get; }

    public ErrorOutput(string code, string message, string detailedMessage)
    {
        Code = code;
        Message = message;
        DetailedMessage = detailedMessage;
    }

}