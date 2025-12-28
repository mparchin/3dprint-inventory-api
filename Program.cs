using _3dprint_inventory_api;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddDotNetEnv(options: new(clobberExistingVars: false));

builder.Services.AddDbContext<Db>(options =>
    options.UseSqlite($"Data Source={builder.Configuration.GetDbPath()}"));

var app = builder.Build();
await app.Services.MigrateAsync();


app.MapGet("/", () => "Hello World!");


app.Run("http://0.0.0.0:8000");
