namespace Podcast.Updater.Worker;

internal sealed class Worker : BackgroundService
{
    private readonly TimeSpan _delay = TimeSpan.FromHours(1);
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _services;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory services)
    {
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var handler = scope.ServiceProvider.GetRequiredService<IPodcastUpdateHandler>();
                await handler.HandleUpdateAsync(stoppingToken);
            }

            await Task.Delay(_delay, stoppingToken);
        }
    }
}