

namespace _3dprint_inventory_api.Schema;

public class Category : IBaseSchema<Category, Models.Category>
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = "";
    public Models.Category ToDbModel() => new()
    {
        CategoryId = CategoryId,
        Name = Name,
    };

    public static Category FromDbModel(Models.Category model) => new()
    {
        CategoryId = model.CategoryId,
        Name = model.Name,
    };
}