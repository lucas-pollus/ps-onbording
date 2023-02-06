using FastEndpoints;
using PS.Web.Models;
using PS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.ConfigureDatabaseServices();
builder.Services.AddRepositoriosServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseFastEndpoints(c =>
    {
        c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
        {
            return new ErrorOutput(code: $"{statusCode}", "Um ou mais erros ocorreram.", detailedMessage: failures.Reduce());
        };
    });

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.Run();
