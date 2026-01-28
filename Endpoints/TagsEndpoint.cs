using _3dprint_inventory_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api.Endpoints;

public static class TagsEndpoint
{
    public static WebApplication MapTagsEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("tags");

        group.MapGet("/", GetAllAsync);
        group.MapGet("/report", GetReportAsync);
        group.MapDelete("/{tagId}", DeleteAsync);
        group.MapPut("/{tagId}", EditAsync);

        return app;
    }
    private static async Task<Ok<List<Tag>>> GetAllAsync(Db db) =>
        TypedResults.Ok(await db.Tags
            .AsNoTracking()
            .Include(t => t.ModelTags)
            .ToListAsync());

    private static async Task<Ok<List<Tag>>> GetReportAsync(Db db)
    {
        var models = await db.Models
            .Include(m => m.ModelTags)
            .Include("ModelTags.Tag")
            .Include(m => m.Files)
            .Include("Files.FileType")
            .ToListAsync();

        return TypedResults.Ok(models.SelectMany(m => m.ModelTags.Select(mt => mt.Tag))
            .DistinctBy(t => t.TagId)
            .ToList());
    }

    private static async Task<Results<Ok<Tag>, NotFound, BadRequest>> EditAsync(Db db, int tagId, Tag tag)
    {
        if (tagId != tag.TagId)
            return TypedResults.BadRequest();

        if (await db.Tags.AllAsync(t => t.TagId != tagId))
            return TypedResults.NotFound();

        db.Tags.Update(tag);

        await db.SaveChangesAsync();
        return TypedResults.Ok(tag);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteAsync(Db db, int tagId)
    {
        var model = await db.Tags
            .FirstOrDefaultAsync(t => t.TagId == tagId);
        if (model is null)
            return TypedResults.NotFound();

        db.Tags.Remove(model);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}