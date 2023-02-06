using PS.Web.Features.CategoriasProdutos;

namespace PS.Web.Services;

public static partial class ServicesExtensions
{
    public static void AddRepositoriosServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositorioCategoriasProdutos, RepositorioCategoriasProdutos>();
    }
}