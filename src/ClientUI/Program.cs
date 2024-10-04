namespace ClientUI;

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
            .ConfigureLogging(cfg => cfg.SetMinimumLevel(LogLevel.Warning))
            .ConfigureServices((_, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumers(Assembly.GetExecutingAssembly());

                    x.SetupTransport(args);
                });

                services.AddSingleton<SimulatedCustomers>();
                services.AddHostedService<SimulatedCustomers>();
                services.AddHostedService<ConsoleBackgroundService>();
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Load (ClientUI)";

        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}