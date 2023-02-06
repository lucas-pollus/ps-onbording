namespace PS.Data;

public sealed class ConnectionString
{
    public string Value { get; private set; }

    public ConnectionString(string value)
    {
        Value = value;
    }
}