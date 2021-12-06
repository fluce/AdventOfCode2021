using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Visualize.Client;
using Visualize.Shared;
using SignalR.Strong;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<HubConnection>(sp => {
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
      .WithUrl(navigationManager.ToAbsoluteUri("/eventhub"))
      .WithAutomaticReconnect()
      .Build();
});

builder.Services.AddSingleton<ICSEventHub>(sp => {
    var conn = sp.GetRequiredService<HubConnection>();
    return conn.AsDynamicHub<ICSEventHub>();
});


await builder.Build().RunAsync();
