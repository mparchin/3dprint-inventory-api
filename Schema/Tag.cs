
namespace _3dprint_inventory_api.Schema;

public class Tag : IBaseSchema<Tag, Models.Tag>
{
    public int TagId { get; set; }
    public string Name { get; set; } = "";
    public Models.Tag ToDbModel() => new()
    {
        TagId = TagId,
        Name = Name,
    };

    public static Tag FromDbModel(Models.Tag model) => new()
    {
        TagId = model.TagId,
        Name = model.Name,
    };

}