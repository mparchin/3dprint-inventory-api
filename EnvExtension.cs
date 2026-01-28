namespace _3dprint_inventory_api;

public static class EnvExtension
{
    public static string GetDbPath(this IConfiguration configuration) =>
        Path.Join(configuration.GetValue("Db_Path", "Data/"), "api.db");
    public static string GetFilePath(this IConfiguration configuration) =>
        configuration.GetValue("Model_Path", "Data/");
    public static string GetTempFilePath(this IConfiguration configuration) =>
        configuration.GetValue("Model_Temp_Path", "Data/tmp/");
    public static Uri GetHost(this IConfiguration configuration) =>
        new(configuration.GetValue("Host", "http://localhost:8000"));
    public static Uri GetFrontHost(this IConfiguration configuration) =>
        new(configuration.GetValue("Front_Host", "http://localhost:3000"));
    public static Uri GetMediaHost(this IConfiguration configuration) =>
        new(configuration.GetValue("Media_Host", "http://localhost:8000"));
    public static Uri GetSpoolmanHost(this IConfiguration configuration) =>
        new(configuration.GetValue("Spoolman_Host", "https://spoolman.ho.me"));
    public static TimeSpan GetTempFileExpiry(this IConfiguration configuration) =>
        TimeSpan.FromMinutes(configuration.GetValue("Model_Temp_Expiry_m", 10));
    public static TimeSpan GetRefreshRate(this IConfiguration configuration) =>
        TimeSpan.FromSeconds(configuration.GetValue("Refresh_Rate_s", 60));

    public static string GetFileUrlFromPath(this Models.File file)
    {
        var mediaHost = new Uri(Environment.GetEnvironmentVariable("Media_Host") ?? "http://localhost:8000");
        var modelPath = Environment.GetEnvironmentVariable("Model_Path") ?? "Data/";
        var relativeUrl = modelPath.EndsWith('/') ? file.Path.Replace(modelPath, "mdl/") : file.Path.Replace(modelPath, "mdl");
        return new Uri(mediaHost, relativeUrl).AbsoluteUri;
    }

    public static string GetTempFileUrlFromPath(string path)
    {
        var mediaHost = new Uri(Environment.GetEnvironmentVariable("Media_Host") ?? "http://localhost");
        var modelPath = Environment.GetEnvironmentVariable("Model_Temp_Path") ?? "Data/tmp/";
        var relativeUrl = modelPath.EndsWith('/') ? path.Replace(modelPath, "tmp/") : path.Replace(modelPath, "tmp");
        return new Uri(mediaHost, relativeUrl).AbsoluteUri;
    }

}