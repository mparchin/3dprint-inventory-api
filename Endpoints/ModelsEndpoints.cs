using _3dprint_inventory_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api.Endpoints;

public static class ModelsEndpoint
{
    public static WebApplication MapModelsEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("models");

        group.MapGet("/", GetAllAsync);
        group.MapPost("/", CreateAsync);
        group.MapGet("/{modelId}", GetAsync);
        group.MapPut("/{modelId}", EditAsync);
        group.MapDelete("/{modelId}", DeleteAsync);

        return app;
    }
    private static async Task<Ok<List<Model>>> GetAllAsync(Db db) =>
        TypedResults.Ok(await db.Models
            .AsNoTracking()
            .Include(m => m.Tags)
            .Include(m => m.Files)
            .Include(m => m.Category)
            .ToListAsync());
    private static async Task<Created<Model>> CreateAsync(Db db, Model model)
    {
        await db.Models.AddAsync(model);
        await db.SaveChangesAsync();
        return TypedResults.Created($"models/{model.ModelId}", model);
    }
    private static async Task<Results<Ok<Model>, NotFound>> GetAsync(Db db, int modelId)
    {
        var model = await db.Models
            .AsNoTracking()
            .Include(m => m.Tags)
            .Include(m => m.Files)
            .Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.ModelId == modelId);
        return model is null ? TypedResults.NotFound() : TypedResults.Ok(model);
    }
    private static async Task<Results<Ok<Model>, NotFound, BadRequest>> EditAsync(Db db, int modelId, Model model)
    {
        if (modelId != model.ModelId)
            return TypedResults.BadRequest();
        if (!await db.Models.AnyAsync(m => m.ModelId == modelId))
            return TypedResults.NotFound();
        db.Models.Update(model);
        model.UpdatedOn = DateTime.Now;
        await db.SaveChangesAsync();
        return TypedResults.Ok(model);
    }
    private static async Task<Results<NoContent, NotFound>> DeleteAsync(Db db, int modelId)
    {
        var model = await db.Models
            .Include(m => m.Tags)
            .Include(m => m.Files)
            .Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.ModelId == modelId);
        if (model is null)
            return TypedResults.NotFound();
        db.Models.Remove(model);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}