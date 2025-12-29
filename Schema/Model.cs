namespace _3dprint_inventory_api.Schema;

public class Model : IBaseSchema<Model, Models.Model>
{
    public int ModelId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public Category Category { get; set; } = new();
    public List<File> Files { get; set; } = [];
    public List<Tag> Tags { get; set; } = [];

    public static Model FromDbModel(Models.Model model) => new()
    {
        ModelId = model.ModelId,
        Name = model.Name,
        Description = model.Description,
        ShortDescription = model.ShortDescription,
        Category = Category.FromDbModel(model.Category),
        Files = [.. model.Files.Select(File.FromDbModel)],
        Tags = [.. model.Tags.Select(Tag.FromDbModel)],
    };

    public Models.Model ToDbModel() => new()
    {
        ModelId = ModelId,
        Name = Name,
        Description = Description,
        ShortDescription = ShortDescription,
        Category = Category.ToDbModel(),
        Files = [.. Files.Select(f => f.ToDbModel())],
        Tags = [.. Tags.Select(t => t.ToDbModel())]
    };
}