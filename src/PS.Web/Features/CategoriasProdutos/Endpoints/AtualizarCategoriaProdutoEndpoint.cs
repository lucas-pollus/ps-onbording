using FastEndpoints;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Features.CategoriasProdutos.Endpoints;

public class AtualizarCategoriaProdutoEndpoint : Endpoint<AtualizarCategoriaProdutoRequest>
{
    private readonly IRepositorioCategoriasProdutos repo;

    public AtualizarCategoriaProdutoEndpoint(IRepositorioCategoriasProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Put("/api/v1/categorias-produtos/{CategoriaProdutoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AtualizarCategoriaProdutoRequest req, CancellationToken ct)
    {
        var result = await repo.Atualizar(req.CategoriaProdutoId, req);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendNoContentAsync(ct);
    }
}

public record AtualizarCategoriaProdutoRequest
{
    public int CategoriaProdutoId { get; init; }
    public string Descricao { get; set; }

    public static implicit operator AtualizarCategoriaProdutoCommand(AtualizarCategoriaProdutoRequest req)
    {
        return new()
        {
            Descricao = req.Descricao
        };
    }
}