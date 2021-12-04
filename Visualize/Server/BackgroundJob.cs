using Microsoft.AspNetCore.SignalR;
using Visualize.Server.Hubs;

public class BackgroundJob : BackgroundService
{
    private readonly IHubContext<EventHub, IEventHub> hubContext;
    private readonly ILogger logger;

    public BackgroundJob(IHubContext<EventHub, IEventHub> hubContext, ILogger<BackgroundJob> logger)
    {
        this.hubContext = hubContext;
        this.logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            string msg = $"Bip {i++}";
            await hubContext.Clients.All.EventOccured(msg);
            logger.LogInformation(msg);
            await Task.Delay(1000, stoppingToken);
        }
    }
}