using _3dprint_inventory_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api.Endpoints;

public static class CategoriesEndpoint
{
    public static WebApplication MapCategoriesEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("categories");

        group.MapGet("/", GetAllAsync);
        group.MapGet("/report", GetAllReportAsync);
        group.MapPut("/{categoryId}", EditAsync);
        group.MapDelete("/{categoryId}", DeleteAsync);

        return app;
    }
    private static async Task<Ok<List<Category>>> GetAllAsync(Db db) =>
        TypedResults.Ok(await db.Categories
            .AsNoTracking()
            .Include(c => c.Models)
            .ToListAsync());

    private static async Task<Ok<List<Category>>> GetAllReportAsync(Db db)
    {
        var models = await db.Models
            .Include(m => m.Category)
            .Include(m => m.Files)
            .Include("Files.FileType")
            .ToListAsync();

        return TypedResults.Ok(models.Select(m => m.Category)
            .DistinctBy(c => c.CategoryId)
            .ToList());
    }


    private static async Task<Results<Ok<Category>, NotFound, BadRequest>> EditAsync(Db db, int categoryId, Category category)
    {
        if (categoryId != category.CategoryId)
            return TypedResults.BadRequest();

        if (await db.Categories.AllAsync(c => c.CategoryId != categoryId))
            return TypedResults.NotFound();

        category.UpdatedOn = DateTime.Now;
        db.Categories.Update(category);

        await db.SaveChangesAsync();
        return TypedResults.Ok(category);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteAsync(Db db, int categoryId)
    {
        var model = await db.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        if (model is null)
            return TypedResults.NotFound();
        db.Categories.Remove(model);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}