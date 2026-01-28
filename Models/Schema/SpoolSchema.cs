using System.Text.Json.Serialization;

namespace _3dprint_inventory_api.Models.Schema;

public class SpoolSchema
{
    public int Id { get; set; }
    public FilamentSchema Filament { get; set; } = new();
    public double Price { get; set; }
    [JsonPropertyName("remaining_weight")]
    public double RemainingWeight { get; set; }

}

public class FilamentSchema
{
    public string Name { get; set; } = "";
    public VendorSchema Vendor { get; set; } = new();
    public string Material { get; set; } = "";
    [JsonPropertyName("color_hex")]
    public string? ColorHex { get; set; }
    [JsonPropertyName("multi_color_hexes")]
    public string? MultiColorHexes { get; set; }
    public double Weight { get; set; }
}

public class VendorSchema
{
    public string Name { get; set; } = "";
}