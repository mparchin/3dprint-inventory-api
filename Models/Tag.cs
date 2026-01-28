using System.Text.Json.Serialization;

namespace _3dprint_inventory_api.Models;

public class Tag : BaseEntity
{
    public int TagId { get; set; }
    public string Name { get; set; } = "";

    [JsonIgnore]
    public ICollection<ModelTag> ModelTags { get; set; } = [];

    public int ModelCount => ModelTags.Count;
    public int ProductCount => 0;
    public double TotalRawWeight => ModelTags.Sum(mt => mt.Model.TotalRawWeight);

    public double TotalValueWeight => ModelTags.Sum(mt => mt.Model.TotalValueWeight);
    public double TotalInventory => 0;
    public double TotalSells => 0;
}