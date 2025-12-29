namespace _3dprint_inventory_api.Models;

public class Model : BaseEntity
{
    public int ModelId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public Category Category { get; set; } = new();
    public ICollection<Models.File> Files { get; set; } = [];
    public ICollection<Tag> Tags { get; set; } = [];
}