namespace Visualize.Shared;

public interface ISCEventHub
{
    Task EventOccured(string evt);
    Task DisplayGrid(GridData gridData);
}

public interface ICSEventHub {
    Task Next();
}
