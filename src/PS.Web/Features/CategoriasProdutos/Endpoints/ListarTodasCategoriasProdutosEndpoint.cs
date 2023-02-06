using FastEndpoints;
using PS.Web.Features.CategoriasProdutos.Models;
using PS.Web.Models;

namespace PS.Web.Features.CategoriasProdutos.Endpoints;

public class ListarTodasCategoriasProdutosEndpoint : Endpoint<QueryRequest, QueryResponse<ListarTodasCategoriasProdutosResponse>>
{
    private readonly IRepositorioCategoriasProdutos repo;

    public ListarTodasCategoriasProdutosEndpoint(IRepositorioCategoriasProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Get("/api/v1/categorias-produtos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QueryRequest req, CancellationToken ct)
    {
        var result = await repo.ListarTodas(req);
        if (result.IsFailure)
            ThrowError(result.Error);

        var response = result.Value.ToResponse<ListarTodasCategoriasProdutosOutput, ListarTodasCategoriasProdutosResponse>();

        await SendOkAsync(response, ct);

    }
}

public record ListarTodasCategoriasProdutosResponse
{
    public int Id { get; init; }
    public string Descricao { get; set; }

    public static implicit operator ListarTodasCategoriasProdutosResponse(ListarTodasCategoriasProdutosOutput dto)
    {
        return new()
        {
            Id = dto.Id,
            Descricao = dto.Descricao
        };
    }
}