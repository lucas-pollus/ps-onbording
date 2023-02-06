using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Endpoints;
using PS.Web.Features.CategoriasProdutos.Models;

namespace PS.Web.Test.Features.CategoriasProdutos;

public class ObterCategoriaProdutoPorIdEndpointTest : BaseTest
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
    public async Task StatusIsOk()
    {
        var response = await client
            .Request("api/v1/categorias-produtos")
            .AppendPathSegment(id)
            .GetAsync();

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var body = await response.GetJsonAsync<ObterCategoriaProdutoPorIdResponse>();
        body.Id.Should().Be(id);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await repo.Remover(id);
    }
}