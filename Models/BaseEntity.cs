using System.Text.Json.Serialization;

namespace _3dprint_inventory_api.Models;

public abstract class BaseEntity
{
    [JsonIgnore]
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    [JsonIgnore]
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
}