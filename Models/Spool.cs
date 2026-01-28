using System.ComponentModel.DataAnnotations.Schema;

namespace _3dprint_inventory_api.Models;

public class Spool : BaseEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int SpoolId { get; set; }
    public string FilamentName { get; set; } = "";
    public string VendorName { get; set; } = "";
    public string Material { get; set; } = "";
    public string? ColorHex { get; set; }
    public string? MultiColorHexes { get; set; }
    public double Price { get; set; }
    public double RemainingWeight { get; set; }
    public double FilamentWeight { get; set; } = 1000;
    public double RemainingValue => Price * RemainingWeight / (FilamentWeight != 0 ? FilamentWeight : 1000);
}