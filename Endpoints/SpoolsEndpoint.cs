using _3dprint_inventory_api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api.Endpoints;

public static class SpoolsEndpoint
{
    public static WebApplication MapSpoolsEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("spools");

        group.MapGet("/", GetAllAsync);
        group.MapGet("/lowestPricePerG", GetLowestSpoolCostPerG);
        group.MapPost("/sync", SyncAsync);

        return app;
    }
    private static async Task<Ok<List<Spool>>> GetAllAsync(Db db) =>
        TypedResults.Ok(await db.Spools
            .AsNoTracking()
            .ToListAsync());

    private static async Task<Ok<double>> GetLowestSpoolCostPerG(Db db) =>
        TypedResults.Ok(await db.Spools.Select(s => s.Price / s.FilamentWeight)
            .OrderBy(s => s)
            .FirstOrDefaultAsync());

    private static async Task<Ok<Spool[]>> SyncAsync(Db db, [FromBody] Spool[] spools)
    {
        var existingIds = await db.Spools
                .AsNoTracking()
                .Select(s => s.SpoolId)
                .ToListAsync();

        spools.Where(s => existingIds.All(eid => eid != s.SpoolId))
            .ToList()
            .ForEach(db.Spools.Add);

        spools.Where(s => existingIds.Contains(s.SpoolId))
            .ToList()
            .ForEach(db.Spools.Update);

        await db.SaveChangesAsync();

        return TypedResults.Ok(spools);
    }

}