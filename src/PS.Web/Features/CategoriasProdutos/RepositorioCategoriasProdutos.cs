using PS.Data;
using PS.Data.Models;
using PS.FluentResult;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Features.CategoriasProdutos;

public sealed class RepositorioCategoriasProdutos : IRepositorioCategoriasProdutos
{
    private readonly IDbHelper dbHelper;

    public RepositorioCategoriasProdutos(IDbHelper dbHelper)
    {
        this.dbHelper = dbHelper;
    }
    public Task<Result> Atualizar(int id, AtualizarCategoriaProdutoCommand cmd)
    {
        return this.dbHelper.ExecuteAsync(
            "app_categorias_produtos.atualizar",
            new
            {
                pi_id = id,
                pi_descricao = cmd.Descricao
            }
        );
    }

    public Task<Result<CategoriaProdutoOutput>> Criar(CriarCategoriaProdutoCommand cmd)
    {
        return this.dbHelper.CreateAsync<CategoriaProdutoOutput>(
            "app_categorias_produtos.criar",
            new
            {
                pi_descricao = cmd.Descricao
            }
        );
    }

    public Task<Result<QueryOutput<ListarTodasCategoriasProdutosOutput>>> ListarTodas(QueryInput query)
    {
        return this.dbHelper.PagedList<ListarTodasCategoriasProdutosOutput>(
            "app_categorias_produtos.listar_todas",
            new
            {
                pi_page = query.Page,
                pi_page_size = query.PageSize
            }
        );
    }

    public Task<Result<CategoriaProdutoOutput>> ObterPorId(int id)
    {
        return this.dbHelper.GetAsync<CategoriaProdutoOutput>(
            "app_categorias_produtos.obter_por_id",
            new
            {
                pi_id = id
            }
        );
    }

    public Task<Result> Remover(int id)
    {
        return this.dbHelper.ExecuteAsync(
            "app_categorias_produtos.remover",
            new
            {
                pi_id = id
            }
        );
    }
}