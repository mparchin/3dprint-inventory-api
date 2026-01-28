using _3dprint_inventory_api.Models;

namespace _3dprint_inventory_api.Services;

public class TempFilesService(ILogger<TempFilesService> logger,
    IConfiguration config, TempFilesDb db) : BackgroundService
{
    private readonly PeriodicTimer _periodicTimer =
        new(config.GetRefreshRate());

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Started Temp files service");
        while (!stoppingToken.IsCancellationRequested
                    && await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            db.RemoveExpiredFiles();
            db.RemoveUnusedFiles();
        }
    }

    public static FileType GetFileType(string name) => Path.GetExtension(name) switch
    {
        ".apng" => FileType.Image,
        ".png" => FileType.Image,
        ".avif" => FileType.Image,
        ".gif" => FileType.Image,
        ".jpg" => FileType.Image,
        ".jpeg" => FileType.Image,
        ".jfif" => FileType.Image,
        ".pjpeg" => FileType.Image,
        ".pjp" => FileType.Image,
        ".svg" => FileType.Image,
        ".webp" => FileType.Image,
        ".stl" => FileType.STL,
        _ => FileType.Other,
    };

    public Models.File MoveTempToModel(Models.File f, string category, string modelName)
    {
        var fileName = Path.GetFileName(f.Path);
        var directory = Path.Join(config.GetFilePath(), category, modelName);
        Directory.CreateDirectory(directory);
        var newPath = Path.Join(directory, fileName);
        System.IO.File.Move(f.Path, newPath);
        db.TempFiles.RemoveAll(t => t.Path == f.Path);
        return new Models.File
        {
            FileType = GetFileType(fileName),
            Path = newPath,
            Weight = f.Weight,
            Repeatations = f.Repeatations,
            ElectricityCostg = f.ElectricityCostg,
            PrintTime = f.PrintTime,
            Size = f.Size
        };
    }
}