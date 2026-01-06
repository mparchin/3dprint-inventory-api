using System.ComponentModel.DataAnnotations.Schema;

namespace _3dprint_inventory_api.Models;


public class FileType : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int FileTypeId { get; set; }
    public string Name { get; set; } = "";
    public ICollection<Models.File> Files { get; set; } = [];

    public static FileType STL { get; } = new()
    {
        FileTypeId = 1,
        Name = "STL",
    };

    public static FileType Other { get; } = new()
    {
        FileTypeId = 2,
        Name = "Other",
    };

    public static FileType Image { get; } = new()
    {
        FileTypeId = 3,
        Name = "Image",
    };
}