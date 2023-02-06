using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Endpoints;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Test.Features.CategoriasProdutos;

public class AtualizarCategoriaProdutoEndpointTest : BaseTest
{
    private IRepositorioCategoriasProdutos repo;
    private int id;

    [OneTimeSetUp]
    public async Task Setup()
    {
        repo = serviceProvider.GetService<IRepositorioCategoriasProdutos>();
        var cmd = new CriarCategoriaProdutoCommand()
        {
            Descricao = "Mesa de Jantar Test"
        };
        var result = await repo.Criar(cmd);
        result.EnsureSuccess();
        id = result.Value.Id;
    }

    [Test]
    public async Task Success()
    {
        var model = new AtualizarCategoriaProdutoRequest()
        {
            Descricao = "Nova Mesa de Jantar Test"
        };

        var response = await client
            .Request("api/v1/categorias-produtos")
            .AppendPathSegment(id)
            .PutJsonAsync(model);

        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

        var result = await repo.ObterPorId(id);

        result.Value.Descricao.Should().BeEquivalentTo(model.Descricao);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await repo.Remover(id);
    }
}