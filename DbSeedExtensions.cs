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
    }
}