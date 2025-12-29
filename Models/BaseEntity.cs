namespace _3dprint_inventory_api.Models;

public abstract class BaseEntity
{
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
}