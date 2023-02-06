using FastEndpoints;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Features.CategoriasProdutos.Endpoints;

public class ObterCategoriaProdutoPorIdEndpoint : EndpointWithoutRequest<ObterCategoriaProdutoPorIdResponse>
{
    private readonly IRepositorioCategoriasProdutos repo;

    public ObterCategoriaProdutoPorIdEndpoint(IRepositorioCategoriasProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Get("/api/v1/categorias-produtos/{CategoriaProdutoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var categoriaProdutoId = Route<int>("CategoriaProdutoId");
        var result = await repo.ObterPorId(categoriaProdutoId);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendOkAsync(result.Value, ct);
    }
}

public record ObterCategoriaProdutoPorIdResponse
{
    public int Id { get; init; }
    public string Descricao { get; init; }

    public static implicit operator ObterCategoriaProdutoPorIdResponse(CategoriaProdutoOutput dto)
    {
        return new()
        {
            Id = dto.Id,
            Descricao = dto.Descricao
        };
    }
}