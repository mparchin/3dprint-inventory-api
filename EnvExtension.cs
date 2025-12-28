namespace _3dprint_inventory_api;

public static class EnvExtension
{
    public static string GetDbPath(this IConfiguration configuration) =>
        Path.Join(configuration.GetValue("Db_Path", "Data/"), "api.db");
}