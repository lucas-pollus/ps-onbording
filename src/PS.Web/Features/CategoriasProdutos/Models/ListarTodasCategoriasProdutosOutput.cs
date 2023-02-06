using PS.Data.Models;

namespace PS.Web.Features.CategoriasProdutos.Models;

public record ListarTodasCategoriasProdutosOutput : IQueryOutput
{
    public int Id { get; init; }
    public string Descricao { get; set; }
}