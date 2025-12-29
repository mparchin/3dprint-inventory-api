namespace _3dprint_inventory_api.Models;

public class File : BaseEntity
{
    public int FileId { get; set; }
    public FileType FileType { get; set; } = FileType.STL;
    public string RelativePath { get; set; } = "";
    public double Mass { get; set; }

    public Model Model { get; set; } = new();
}