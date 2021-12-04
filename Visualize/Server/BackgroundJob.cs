using Microsoft.AspNetCore.SignalR;
using Visualize.Server.Hubs;
using Visualize.Shared;

public class BackgroundJob : BackgroundService
{
    private readonly IHubContext<EventHub, IEventHub> hubContext;
    private readonly NextHandler nextHandler;
    private readonly ILogger logger;

    public BackgroundJob(IHubContext<EventHub, IEventHub> hubContext, NextHandler nextHandler, ILogger<BackgroundJob> logger)
    {
        this.hubContext = hubContext;
        this.nextHandler = nextHandler;
        this.logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(async () =>
        {
            int i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                string msg = $"Bip {i++}";
                await hubContext.Clients.All.EventOccured(msg);
                await hubContext.Clients.All.DisplayGrid(GridData.From(16, 16, (x, y) => Random.Shared.Next(2) == 0));
                logger.LogInformation(msg);
                await Task.WhenAny(nextHandler.WaitForNext(),Task.Delay(10000,stoppingToken));
            }
        },stoppingToken);
    }
}