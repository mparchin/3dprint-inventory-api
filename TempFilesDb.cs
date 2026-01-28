using _3dprint_inventory_api.Models;

namespace _3dprint_inventory_api;

public class TempFilesDb(IConfiguration configuration)
{
    public List<TempFile> TempFiles { get; } = [];

    public void RemoveUnusedFiles() =>
        Directory.GetFiles(configuration.GetTempFilePath())
            .Where(f => TempFiles.All(t => t.Path != f))
            .ToList()
            .ForEach(System.IO.File.Delete);

    public void RemoveExpiredFiles() =>
        TempFiles.RemoveAll(f => f.Expiry <= DateTime.Now);
}