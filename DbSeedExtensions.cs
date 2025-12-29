using _3dprint_inventory_api.Models;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api;

public static class DbSeedExtension
{
    public static async Task MigrateAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var config = services.GetRequiredService<IConfiguration>();
        Directory.CreateDirectory(new FileInfo(config.GetDbPath()).Directory!.FullName);
        var db = scope.ServiceProvider.GetRequiredService<Db>();
        await db.Database.MigrateAsync();

        await db.SeedFileTypesAsync();

        // var cat = ;
        var model = new Model()
        {
            Category = new Category()
            {
                Name = "Test",
            }
        };

        // await db.Models.AddAsync(model);

        await db.SaveChangesAsync();
    }

    public static async Task SeedFileTypesAsync(this Db db)
    {
        var fileTypes = await db.FileTypes.ToListAsync();
        if (fileTypes.All(f => f.FileTypeId != FileType.Cura.FileTypeId))
            await db.FileTypes.AddAsync(FileType.Cura);
        if (fileTypes.All(f => f.FileTypeId != FileType.STL.FileTypeId))
            await db.FileTypes.AddAsync(FileType.STL);
        if (fileTypes.All(f => f.FileTypeId != FileType.Image.FileTypeId))
            await db.FileTypes.AddAsync(FileType.Image);
    }
}