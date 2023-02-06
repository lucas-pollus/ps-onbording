using PS.Web.Features.CategoriasProdutos;
using PS.Web.Features.CategoriasProdutos.Endpoints;
using PS.Web.Models;

namespace PS.Web.Test.Features.CategoriasProdutos;

public class CriarCategoriaProdutoEndpointTest : BaseTest
{
    private int id;

    [Test]
    public async Task StatusIsOk()
    {
        var model = new CriarCategoriaProdutoRequest()
        {
            Descricao = "Mesa de Jantar Test"
        };

        var response = await client
            .Request("api/v1/categorias-produtos")
            .PostJsonAsync(model);

        response.StatusCode.Should().Be((int)HttpStatusCode.OK);

        var body = await response
            .GetJsonAsync<CriarCategoriaProdutoResponse>();

        id = body.Id;

        body.Descricao.Should().BeEquivalentTo(model.Descricao);
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        var repo = serviceProvider.GetService<IRepositorioCategoriasProdutos>();
        await repo.Remover(id);
    }
}

public class FalhaAoCriarCategoriaProdutoEndpointTest : BaseTest
{
    [Test]
    public async Task StatusIsBadRequest()
    {
        var modelInvalido = new CriarCategoriaProdutoRequest()
        {
            Descricao = "AB"
        };

        var response = await client
            .Request("api/v1/categorias-produtos")
            .AllowAnyHttpStatus()
            .PostJsonAsync(modelInvalido);

        response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

        var body = await response.GetJsonAsync<ErrorOutput>();
        body.DetailedMessage.Should().ContainAny("A descrição deve ter no mínimo 3 caracteres.");
    }
}