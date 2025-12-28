namespace _3dprint_inventory_api.Models;

public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; } = "";

    public ICollection<Model> Models { get; set; } = [];
}