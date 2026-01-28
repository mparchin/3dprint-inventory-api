using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace _3dprint_inventory_api.Models;

[PrimaryKey("ModelId", "TagId")]
[Index("ModelId", "TagId", IsUnique = true)]
public class ModelTag
{
    [JsonIgnore]
    public Model Model { get; set; } = new();
    public Tag Tag { get; set; } = new();
}