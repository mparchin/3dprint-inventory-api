using System.ComponentModel.DataAnnotations;

namespace _3dprint_inventory_api.Models;

public class Model : BaseEntity
{
    public int ModelId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public string PrintSpecifications { get; set; } = "";
    [Required]
    public Category Category { get; set; } = new();
    [Required]
    public ICollection<File> Files { get; set; } = [];
    [Required]
    public ICollection<ModelTag> ModelTags { get; set; } = [];
    public double? AdditionalCostsg { get; set; }
    public double ValueToCostRatio { get; set; }

    public double TotalRawWeight => Files.Where(f => f.FileType.FileTypeId == FileType.STL.FileTypeId)
        .Sum(f => (f.Weight ?? 0) * (f.Repeatations ?? 1));
    public double TotalValueWeight => (Files.Where(f => f.FileType.FileTypeId == FileType.STL.FileTypeId)
        .Sum(f => ((f.Weight ?? 0) + (f.ElectricityCostg ?? 0)) * (f.Repeatations ?? 1)) + (AdditionalCostsg ?? 0)) * ValueToCostRatio;

}