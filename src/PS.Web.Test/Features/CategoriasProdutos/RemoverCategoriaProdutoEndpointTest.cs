using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Test.Features.CategoriasProdutos;

public class RemoverCategoriaProdutoEndpointTest : BaseTest
{
    private int id;

    [OneTimeSetUp]
    public async Task Setup()
    {
        var repo = serviceProvider.GetService<IRepositorioCategoriasProdutos>();

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
        var response = await client
            .Request("api/v1/categorias-produtos")
            .AppendPathSegment(id)
            .DeleteAsync();

        response.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
    }
}