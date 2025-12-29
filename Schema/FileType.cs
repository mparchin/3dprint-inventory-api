namespace _3dprint_inventory_api.Schema;

public class FileType : IBaseSchema<FileType, Models.FileType>
{
    public int FileTypeId { get; set; }
    public string Name { get; set; } = "";

    public static FileType FromDbModel(Models.FileType model) => new()
    {
        Name = model.Name,
        FileTypeId = model.FileTypeId,
    };

    public Models.FileType ToDbModel() => new()
    {
        Name = Name,
        FileTypeId = FileTypeId,
    };
}