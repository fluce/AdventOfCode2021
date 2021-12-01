using Microsoft.AspNetCore.SignalR;

namespace Visualize.Server.Hubs;

public interface IEventHub
{
    Task EventOccured(string evt);
}

public class EventHub: Hub<IEventHub>
{

}
