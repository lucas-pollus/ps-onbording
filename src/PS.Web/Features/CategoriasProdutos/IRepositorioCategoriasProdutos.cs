using PS.Data.Models;
using PS.FluentResult;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Features.CategoriasProdutos;

public interface IRepositorioCategoriasProdutos
{
    Task<Result<CategoriaProdutoOutput>> Criar(CriarCategoriaProdutoCommand cmd);
    Task<Result> Atualizar(int id, AtualizarCategoriaProdutoCommand cmd);
    Task<Result<CategoriaProdutoOutput>> ObterPorId(int id);
    Task<Result> Remover(int id);
    Task<Result<QueryOutput<ListarTodasCategoriasProdutosOutput>>> ListarTodas(QueryInput query);
}