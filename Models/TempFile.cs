namespace _3dprint_inventory_api.Models;

public record TempFile(
    string Name,
    string Path,
    FileType FileType,
    string Url,
    DateTime Expiry);