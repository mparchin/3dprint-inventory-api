using System.Text.Json.Serialization;
using _3dprint_inventory_api;
using _3dprint_inventory_api.Endpoints;
using _3dprint_inventory_api.Models;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options => options.Limits.MaxRequestBodySize = 300 * 1024 * 1024);
builder.Configuration.AddDotNetEnv(options: new(clobberExistingVars: false));

builder.Services.AddDbContext<Db>(options =>
    options.UseSqlite($"Data Source={builder.Configuration.GetDbPath()}"));

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// builder.Services.AddAntiforgery();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder.WithOrigins("http://localhost:3000", "http://localhost:8000")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "3dprint-inventory-api";
    config.Title = "3dprint-inventory-api v1";
    config.Version = "v1";
});

var app = builder.Build();
await app.Services.MigrateAsync();

// app.UseAntiforgery();
app.UseCors();
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

app.MapPost("/files", async (IFormFileCollection collection, IConfiguration config) =>
{
    collection.ToList().ForEach(f => Console.WriteLine($"{f.FileName}\t{f.Name}\t{f.Length}\t{f.ContentDisposition}\t{f.ContentType}"));
    //addUser as folder
    var path = config.GetTempFilePath();
    Directory.CreateDirectory(path);
    var tasks = collection.Select(async f =>
    {
        var filePath = Path.Join(path, f.FileName);
        using var stream = System.IO.File.OpenWrite(filePath);
        await f.CopyToAsync(stream);
        return new TempFile(f.FileName, filePath, $"http//localhost:8000/{filePath}");
    });
    await Task.WhenAll(tasks);
    return TypedResults.Ok(tasks.Select(t => t.Result));
}).DisableAntiforgery();


app.Run("http://0.0.0.0:8000");
