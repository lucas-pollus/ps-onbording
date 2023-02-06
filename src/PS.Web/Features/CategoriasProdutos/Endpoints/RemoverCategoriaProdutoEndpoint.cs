using FastEndpoints;

namespace PS.Web.Features.CategoriasProdutos.Endpoints;

public class RemoverCategoriaProdutoEndpoint : EndpointWithoutRequest
{
    private readonly IRepositorioCategoriasProdutos repo;

    public RemoverCategoriaProdutoEndpoint(IRepositorioCategoriasProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Delete("/api/v1/categorias-produtos/{CategoriaProdutoId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var categoriaProdutoId = Route<int>("CategoriaProdutoId");
        var result = await repo.Remover(categoriaProdutoId);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendNoContentAsync(ct);
    }
}