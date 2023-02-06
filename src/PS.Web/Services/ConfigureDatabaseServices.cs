using System.Data;
using Oracle.ManagedDataAccess.Client;
using PS.Data;
using PS.Web.Features.CategoriasProdutos;

namespace PS.Web.Services;

public static partial class ServicesExtensions
{
    public static void ConfigureDatabaseServices(this WebApplicationBuilder builder)
    {
        var conn = builder.Configuration["ConnectionStringDb"];
        builder.Services.AddSingleton(new ConnectionString(conn));
        builder.Services.AddScoped<IDbConnection, OracleConnection>();
        builder.Services.AddScoped<IDbHelper, DbHelper>();
    }
}