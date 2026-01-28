using _3dprint_inventory_api.Models;
using _3dprint_inventory_api.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace _3dprint_inventory_api.Endpoints;

public static class FilesEndpoint
{
    public static WebApplication MapFilesEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("files");

        group.MapPost("/", UploadFile).DisableAntiforgery();

        return app;
    }

    public static async Task<Ok<IEnumerable<TempFile>>> UploadFile(IFormFileCollection collection, IConfiguration config, TempFilesDb db)
    {
        var path = config.GetTempFilePath();
        //add user to path
        Directory.CreateDirectory(path);
        var tasks = collection.Select(async f =>
        {
            var filePath = Path.Join(path, f.FileName);

            var ret = new TempFile(f.FileName, filePath, TempFilesService.GetFileType(f.FileName),
                EnvExtension.GetTempFileUrlFromPath(filePath),
                DateTime.Now + config.GetTempFileExpiry());
            db.TempFiles.Add(ret);

            await using var stream = System.IO.File.OpenWrite(filePath);
            await f.CopyToAsync(stream);
            return ret;
        });
        await Task.WhenAll(tasks);
        return TypedResults.Ok(tasks.Select(t => t.Result));
    }

}