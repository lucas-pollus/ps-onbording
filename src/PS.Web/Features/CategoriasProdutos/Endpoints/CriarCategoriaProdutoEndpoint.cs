using FastEndpoints;
using FluentValidation;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Features.CategoriasProdutos.Endpoints;

public class CriarCategoriaProdutoEndpoint : Endpoint<CriarCategoriaProdutoRequest, CriarCategoriaProdutoResponse>
{
    private readonly IRepositorioCategoriasProdutos repo;

    public CriarCategoriaProdutoEndpoint(IRepositorioCategoriasProdutos repo)
    {
        this.repo = repo;
    }

    public override void Configure()
    {
        Post("/api/v1/categorias-produtos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CriarCategoriaProdutoRequest req, CancellationToken ct)
    {
        var result = await repo.Criar(req);
        if (result.IsFailure)
            ThrowError(result.Error);

        await SendOkAsync(result.Value, ct);
    }
}

public record CriarCategoriaProdutoRequest
{
    public string Descricao { get; init; }

    public static implicit operator CriarCategoriaProdutoCommand(CriarCategoriaProdutoRequest req)
    {
        return new()
        {
            Descricao = req.Descricao
        };
    }
}

public class CriarCategoriaProdutoValidator : Validator<CriarCategoriaProdutoRequest>
{
    public CriarCategoriaProdutoValidator()
    {
        RuleFor(p => p.Descricao)
            .NotEmpty()
            .WithMessage("A descrição não pode ser vazio.")
            .MinimumLength(3)
            .WithMessage("A descrição deve ter no mínimo 3 caracteres.")
            .MaximumLength(100)
            .WithMessage("A descrição deve ter no máximo 100 caracteres.");
    }
}

public record CriarCategoriaProdutoResponse
{
    public int Id { get; init; }
    public string Descricao { get; init; }

    public static implicit operator CriarCategoriaProdutoResponse(CategoriaProdutoOutput dto)
    {
        return new()
        {
            Id = dto.Id,
            Descricao = dto.Descricao
        };
    }
}