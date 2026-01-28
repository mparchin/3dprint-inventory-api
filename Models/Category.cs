using System.Text.Json.Serialization;

namespace _3dprint_inventory_api.Models;

public class Category : BaseEntity
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = "";
    public int ModelCount => Models.Count;
    public int ProductCount => 0;
    public double TotalRawWeight => Models.Sum(m => m.TotalRawWeight);

    public double TotalValueWeight => Models.Sum(m => m.TotalValueWeight);
    public double TotalInventory => 0;
    public double TotalSells => 0;

    [JsonIgnore]
    public ICollection<Model> Models { get; set; } = [];
}