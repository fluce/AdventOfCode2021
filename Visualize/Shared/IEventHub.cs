namespace Visualize.Shared;

public interface IEventHub
{
    Task EventOccured(string evt);
    Task DisplayGrid(GridData gridData);
}
