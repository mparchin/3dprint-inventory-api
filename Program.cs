using System.Text.Json.Serialization;
using _3dprint_inventory_api;
using _3dprint_inventory_api.Endpoints;
using _3dprint_inventory_api.Services;
using DotNetEnv.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDotNetEnv(options: new(clobberExistingVars: false));
builder.WebHost.UseKestrel(options => options.Limits.MaxRequestBodySize = 100 * 1024 * 1024);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
})
.AddSingleton<TempFilesDb>()
.AddScoped<TempFilesService>()
.AddDbContext<Db>(options =>
{
    options.EnableSensitiveDataLogging();
    options.UseSqlite($"Data Source={builder.Configuration.GetDbPath()}");
})
.AddCors(options =>
    options.AddDefaultPolicy(b =>
    {
        b.WithOrigins(builder.Configuration.GetFrontHost().AbsoluteUri.TrimEnd('/'),
            builder.Configuration.GetHost().AbsoluteUri.TrimEnd('/'));
        b.AllowAnyHeader();
        b.AllowAnyMethod();
    }))
.AddHostedService<TempFilesService>()
.AddHostedService<SpoolmanService>()
.AddEndpointsApiExplorer()
.AddOpenApiDocument(config =>
{
    config.DocumentName = "3dprint-inventory-api";
    config.Title = "3dprint-inventory-api v1";
    config.Version = "v1";
});

await using var app = builder.Build();
await app.Services.MigrateAsync();

app.UseCors()
    .UseOpenApi()
    .UseSwaggerUi(config =>
    {
        config.DocumentTitle = "3dprint-inventory-api";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.IsPathFullyQualified(app.Configuration.GetFilePath())
                ? app.Configuration.GetFilePath()
                : Path.Join(Environment.CurrentDirectory, app.Configuration.GetFilePath())),
    RequestPath = "/mdl",
    ServeUnknownFileTypes = true,
    HttpsCompression = HttpsCompressionMode.Compress,
});
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.IsPathFullyQualified(app.Configuration.GetTempFilePath())
                ? app.Configuration.GetTempFilePath()
                : Path.Join(Environment.CurrentDirectory, app.Configuration.GetTempFilePath())),
    RequestPath = "/tmp",
    ServeUnknownFileTypes = true,
    HttpsCompression = HttpsCompressionMode.Compress,
});

app.MapGet("/", () => "Hello World!");

await app.MapModelsEndpoint()
    .MapFilesEndpoint()
    .MapCategoriesEndpoint()
    .MapTagsEndpoint()
    .MapSpoolsEndpoint()
    .RunAsync(app.Configuration.GetHost().AbsoluteUri);
