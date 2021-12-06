using Visualize.Server.Hubs;

namespace Visualize.Server;

public class Visualize: IAsyncDisposable
{
    private WebApplication webApplication;

    public Visualize Build(Action<WebApplicationBuilder>? build=null, Action<WebApplication, Action<WebApplication>>? configure=null)
    {
        webApplication=InnerBuild(build,configure);
        return this;
    }

    private WebApplication InnerBuild(Action<WebApplicationBuilder>? build=null, Action<WebApplication, Action<WebApplication>>? configure=null)
    {
        var builder = WebApplication.CreateBuilder();

        // Add services to the container.

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddSignalR();
        builder.Services.AddHostedService<BackgroundJob>();
        builder.Services.AddSingleton<NextHandler>();

        build?.Invoke(builder);

        var app = builder.Build();

        if (configure==null) configure=(a,f)=>f(a);

        configure(app, app=>{ 
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();
            app.MapControllers();

            app.MapHub<EventHub>("/eventhub");

            app.MapFallbackToFile("index.html");


        });
        return app;

    }

    public async Task<Visualize> Start() 
    {
        if (webApplication==null)
            webApplication=InnerBuild();
        await webApplication.StartAsync();
        return this;
    }

    public async ValueTask DisposeAsync() 
    {
        await webApplication.StopAsync();
        await webApplication.DisposeAsync(); 
    }
    
}