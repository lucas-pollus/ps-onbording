namespace PS.Web.Features.CategoriasProdutos.Models;

public record CriarCategoriaProdutoCommand
{
    public string Descricao { get; init; }
}