namespace _3dprint_inventory_api.Models;

public class File : BaseEntity
{
    public int FileId { get; set; }
    public FileType FileType { get; set; } = FileType.Other;
    public string Path { get; set; } = "";
    public double? Weight { get; set; }
    public int? Repeatations { get; set; }
    public double? ElectricityCostg { get; set; }
    public TimeSpan? PrintTime { get; set; }
    public double Size { get; set; }

    public string Url => this.GetFileUrlFromPath();

    public Model Model { get; set; } = new();
}