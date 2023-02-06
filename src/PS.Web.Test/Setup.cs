using Microsoft.AspNetCore.Mvc.Testing;

namespace PS.Web.Test;

[SetUpFixture]
public class Setup
{
    private WebApplicationFactory<Program> webAppFactory;

    [OneTimeSetUp]
    public Task OneTimeSetUp()
    {
        Startup();
        BaseTest.WebAppFactory = webAppFactory;
        return Task.CompletedTask;
    }

    private void Startup() => webAppFactory = new PsWebApp();

    [OneTimeTearDown]
    public void OneTimeTearDown() => webAppFactory?.Dispose();
}