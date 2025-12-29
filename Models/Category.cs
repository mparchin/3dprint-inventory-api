namespace _3dprint_inventory_api.Models;

public class Category : BaseEntity
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = "";

    public ICollection<Model> Models { get; set; } = [];
}