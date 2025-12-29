using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api.Endpoints;

public static class ModelsEndpoint
{
    public static WebApplication MapModelsEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("models/");

        group.MapGet("/", (Db db) => GetAllAsync(db));
        group.MapPost("/", (Db db, Schema.Model model) => CreateAsync(db, model));



        return app;
    }

    private static async Task<Ok<IEnumerable<Schema.Model>>> GetAllAsync(Db db)
    {
        var ret = await db.Models
            .AsNoTracking()
            .Include(m => m.Tags)
            .Include(m => m.Files)
            .Include(m => m.Category)
            .ToListAsync()
            .ContinueWith((task) => task.Result.Select(Schema.Model.FromDbModel));

        return TypedResults.Ok(ret);
    }

    private static async Task<Created<Schema.Model>> CreateAsync(Db db, Schema.Model model)
    {
        var dbModel = model.ToDbModel();
        await db.Models.AddAsync(dbModel);
        await db.SaveChangesAsync();
        return TypedResults.Created($"models/{dbModel.ModelId}", Schema.Model.FromDbModel(dbModel));
    }
}