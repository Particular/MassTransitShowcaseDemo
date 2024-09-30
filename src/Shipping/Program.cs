namespace Shipping;

using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(cfg => cfg.ClearProviders())
            .ConfigureServices((_, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumers(Assembly.GetExecutingAssembly());

                    x.SetupTransport(args);
                });

                services.AddSingleton<SimulationEffects>();
                services.AddHostedService<ConsoleBackgroundService>();
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Processing (Shipping)";

        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}