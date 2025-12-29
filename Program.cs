using _3dprint_inventory_api;
using _3dprint_inventory_api.Endpoints;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddDotNetEnv(options: new(clobberExistingVars: false));

builder.Services.AddDbContext<Db>(options =>
    options.UseSqlite($"Data Source={builder.Configuration.GetDbPath()}"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "3dprint-inventory-api";
    config.Title = "3dprint-inventory-api v1";
    config.Version = "v1";
});

var app = builder.Build();
await app.Services.MigrateAsync();

app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.DocumentTitle = "3dprint-inventory-api";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
});

app.MapGet("/", () => "Hello World!");
app.MapModelsEndpoint();


app.Run("http://0.0.0.0:8000");
