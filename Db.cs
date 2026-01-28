using _3dprint_inventory_api.Models;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api;

public class Db(DbContextOptions options) : DbContext(options)
{
    public DbSet<Models.File> Files { get; set; }
    public DbSet<FileType> FileTypes { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ModelTag> ModelTags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Spool> Spools { get; set; }
}