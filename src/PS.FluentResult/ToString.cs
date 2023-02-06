namespace PS.FluentResult;

public partial struct Result
{
    public override string ToString()
    {
        return IsSuccess ? "Sucesso" : $"Falha({Error})";
    }
}

public partial struct Result<T>
{
    public override string ToString()
    {
        return IsSuccess ? $"Sucesso({Value})" : $"Falha({Error})";
    }
}