
namespace _3dprint_inventory_api.Schema;

public class File : IBaseSchema<File, Models.File>
{
    public int FileId { get; set; }
    public FileType FileType { get; set; } = new();
    public string RelativePath { get; set; } = "";
    public double Mass { get; set; }

    public static File FromDbModel(Models.File model) => new()
    {
        FileId = model.FileId,
        FileType = FileType.FromDbModel(model.FileType),
        RelativePath = model.RelativePath,
        Mass = model.Mass,
    };

    public Models.File ToDbModel() => new()
    {
        FileId = FileId,
        FileType = FileType.ToDbModel(),
        RelativePath = RelativePath,
        Mass = Mass,
    };
}