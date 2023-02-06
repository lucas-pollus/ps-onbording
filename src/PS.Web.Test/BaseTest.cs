using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PS.Web.Test;

public abstract class BaseTest
{
    internal static WebApplicationFactory<Program> WebAppFactory { get; set; }
    private IServiceScope scope;
    protected IServiceProvider serviceProvider;
    protected IConfiguration configuration;
    protected FlurlClient client;

    [OneTimeSetUp]
    public void BaseTestOneTimeSetUp()
    {
        scope = WebAppFactory?.Services.CreateScope();
        serviceProvider = scope?.ServiceProvider;
        configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var defaultClient = WebAppFactory?.CreateDefaultClient();
        client = new FlurlClient(defaultClient);
    }

    [OneTimeTearDown]
    public void BaseTestOneTimeTearDown() => scope?.Dispose();
}