//TODO this has to move to an external project when net is back

using _3dprint_inventory_api.Models;
using _3dprint_inventory_api.Models.Schema;

namespace _3dprint_inventory_api.Services;

public class SpoolmanService : BackgroundService
{
    private readonly HttpClientHandler _handler = new()
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain,
            sslPolicyErrors) => true,
    };
    private readonly HttpClient _httpClient;
    private readonly PeriodicTimer _periodicTimer;
    private readonly ILogger<SpoolmanService> logger;
    private readonly IConfiguration config;

    public SpoolmanService(IConfiguration config,
        ILogger<SpoolmanService> logger)
    {
        _httpClient = new(_handler, true)
        {
            BaseAddress = config.GetSpoolmanHost()
        };
        _periodicTimer = new(config.GetRefreshRate());
        this.logger = logger;
        this.config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Spoolman service started");

        while (!stoppingToken.IsCancellationRequested
            && await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            await SyncWithSpoolManDbAsync(stoppingToken);
        }
    }

    private async Task SyncWithSpoolManDbAsync(CancellationToken stoppingToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(new Uri(
                    _httpClient.BaseAddress!, $"/api/v1/spool"),
                    stoppingToken);
            response.EnsureSuccessStatusCode();
            var spools = await response.Content.ReadFromJsonAsync<SpoolSchema[]>(stoppingToken)
                .ContinueWith(task => task.Result?.Select(s => new Spool
                {
                    SpoolId = s.Id,
                    ColorHex = s.Filament.ColorHex,
                    FilamentName = s.Filament.Name,
                    Material = s.Filament.Material,
                    Price = s.Price,
                    RemainingWeight = s.RemainingWeight,
                    VendorName = s.Filament.Vendor.Name,
                    MultiColorHexes = s.Filament.MultiColorHexes,
                    FilamentWeight = s.Filament.Weight,
                }) ?? []);

            response = await _httpClient.PostAsync(new Uri(
                    config.GetHost(), $"/spools/sync"), JsonContent.Create(spools),
                    stoppingToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }
    }

}