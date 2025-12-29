using System.ComponentModel.DataAnnotations;

namespace _3dprint_inventory_api.Models;

public class Model : BaseEntity
{
    public int ModelId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    [Required]
    public Category Category { get; set; } = new();
    [Required]
    public ICollection<Models.File> Files { get; set; } = [];
    [Required]
    public ICollection<Tag> Tags { get; set; } = [];
}