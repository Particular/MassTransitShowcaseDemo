namespace Billing;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(cfg => cfg.SetMinimumLevel(LogLevel.Warning))
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumers(Assembly.GetExecutingAssembly());
                        x.SetupTransport(args);
                    });

                    services.AddSignalR();
                    services.AddSingleton<SimulationEffects>();
                    services.AddHostedService<ConsoleBackgroundService>();
                });
                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        string url = $"/ServerHub";
                        endpoints.MapHub<MyHub>(url);
                    });
                });
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Failure rate (Billing)";

        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}
