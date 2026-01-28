using _3dprint_inventory_api.Models;
using _3dprint_inventory_api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    private static async Task<Ok<List<Model>>> GetAllAsync(Db db, [FromQuery] int? catId, [FromQuery] int? tagId, [FromQuery] int? skip, [FromQuery] int? take) =>
        TypedResults.Ok(await db.Models
            .AsNoTracking()
            .Where(m => catId == null || m.Category.CategoryId == catId)
            .Where(m => tagId == null || m.ModelTags.Any(t => t.Tag.TagId == tagId))
            .Include(m => m.ModelTags)
            .Include("ModelTags.Tag")
            .Include(m => m.Files)
            .Include(m => m.Category)
            .Include("Files.FileType")
            .Skip(skip ?? 0)
            .Take(take ?? -1)
            .ToListAsync());
    private static async Task<Created<Model>> CreateAsync(Db db, Model model, [FromServices] TempFilesService tempFilesService)
    {
        model.Files = [.. model.Files.Select(f => tempFilesService.MoveTempToModel(f, model.Category.Name, model.Name))];

        if (model.Category.CategoryId != 0)
            db.Categories.Attach(model.Category);

        model.ModelTags.ToList().ForEach(mt => mt.Model = model);
        model.ModelTags
            .DistinctBy(mt => mt.Tag.TagId)
            .ToList()
            .ForEach(mt => db.Tags.Attach(mt.Tag));

        model.Files.Select(f => f.FileType)
            .DistinctBy(ft => ft.FileTypeId)
            .ToList()
            .ForEach(ft => db.FileTypes.Attach(ft));

        model.Files.Where(f => f.FileId != 0)
            .DistinctBy(f => f.FileId)
            .ToList()
            .ForEach(file => db.Files.Update(file));

        db.Models.Add(model);
        await db.SaveChangesAsync();
        return TypedResults.Created($"models/{model.ModelId}", model);
    }
    private static async Task<Results<Ok<Model>, NotFound>> GetAsync(Db db, int modelId)
    {
        var model = await db.Models
            .AsNoTracking()
            .Include(m => m.ModelTags)
            .Include("ModelTags.Tag")
            .Include(m => m.Files)
            .Include("Files.FileType")
            .Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.ModelId == modelId);
        return model is null ? TypedResults.NotFound() : TypedResults.Ok(model);
    }
    private static async Task<Results<Ok<Model>, NotFound, BadRequest>> EditAsync(Db db, int modelId, Model model, [FromServices] TempFilesService tempFilesService)
    {
        if (modelId != model.ModelId)
            return TypedResults.BadRequest();

        if (await db.Models.AllAsync(m => m.ModelId != modelId))
            return TypedResults.NotFound();

        //add new files
        model.Files = [.. model.Files.Select(f => f.FileId == 0 ? tempFilesService.MoveTempToModel(f, model.Category.Name, model.Name) : f)];
        model.Files.ToList()
            .ForEach(f =>
            {
                f.FileType = f.FileType.FileTypeId == FileType.Image.FileTypeId
                    ? FileType.Image
                    : f.FileType.FileTypeId == FileType.STL.FileTypeId ? FileType.STL
                    : FileType.Other;
            });
        model.ModelTags.ToList().ForEach(mt => mt.Model = model);

        model.ModelTags.Where(mt => mt.Tag.TagId == 0)
            .ToList()
            .ForEach(mt => db.Tags.Add(mt.Tag));
        await db.SaveChangesAsync();

        model.UpdatedOn = DateTime.Now;
        db.Models.Update(model);

        //remove deleted files
        await db.Files.Where(f => f.Model.ModelId == modelId)
            .AsNoTracking()
            .Select(f => f.FileId)
            .ToListAsync()
            .ContinueWith(task => task.Result
                .Where(fileId => model.Files.All(mf => mf.FileId != fileId))
                .ToList()
                .ForEach(fileId =>
                {
                    var file = db.Files.Find(fileId)!;
                    db.Files.Remove(file);
                    System.IO.File.Delete(file.Path);
                }));

        var oldTags = db.ModelTags.Where(mt => mt.Model.ModelId == modelId)
            .Include(mt => mt.Tag)
            .AsNoTracking()
            .ToListAsync();

        //new Tags
        await oldTags.ContinueWith(task => model.ModelTags.Where(nmt => task.Result.All(mt => mt.Tag.TagId != nmt.Tag.TagId))
            .ToList()
            .ForEach(nmt => db.ModelTags.Add(nmt)));

        //deleted Tags
        await oldTags.ContinueWith(task => task.Result.Where(mt => model.ModelTags.All(nmt => nmt.Tag.TagId != mt.Tag.TagId))
            .ToList()
            .ForEach(mt => db.ModelTags.Remove(db.ModelTags.Find(modelId, mt.Tag.TagId)!)));

        await db.SaveChangesAsync();
        return TypedResults.Ok(model);
    }
    private static async Task<Results<NoContent, NotFound>> DeleteAsync(Db db, int modelId)
    {
        var model = await db.Models
            .Include(m => m.Files)
            .FirstOrDefaultAsync(m => m.ModelId == modelId);
        if (model is null)
            return TypedResults.NotFound();
        model.Files.ToList()
            .ForEach(f => System.IO.File.Delete(f.Path));
        db.Models.Remove(model);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}