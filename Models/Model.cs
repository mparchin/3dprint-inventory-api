namespace _3dprint_inventory_api.Models;

public class Model
{
    public int ModelId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public ICollection<File> Files { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];

}