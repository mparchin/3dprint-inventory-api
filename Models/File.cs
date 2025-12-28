namespace _3dprint_inventory_api.Models;

public class File
{
    public int FileId { get; set; }
    public FileType Type { get; set; }
    public string RelativePath { get; set; } = "";
    public double Mass { get; set; }


    public int ModelId { get; set; }
    public Model Model { get; set; } = new();
}