using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Channels;
using Visualize.Shared;

namespace Visualize.Server.Hubs;

public class NextHandler
{
    readonly Channel<object?> channel = Channel.CreateUnbounded<object?>();

    public async Task TriggerNext()
    {
        await channel.Writer.WriteAsync(null);
    }

    public async Task WaitForNext()
    {
        await channel.Reader.ReadAsync();
    }
}


public class EventHub: Hub<ISCEventHub>, ICSEventHub
{
    private readonly NextHandler nextHandler;

    public EventHub(NextHandler nextHandler)
    {
        this.nextHandler = nextHandler;
    }

    public async Task Next()
    {
        nextHandler.TriggerNext();
    }

}
