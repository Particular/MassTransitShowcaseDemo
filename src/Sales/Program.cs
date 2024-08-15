#pragma warning disable IDE0010
namespace Sales;

using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host("localhost", "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });

                        cfg.ConfigureEndpoints(context);
                    });

                    x.AddConsumers(Assembly.GetExecutingAssembly());

                    x.AddConfigureEndpointsCallback((name, cfg) =>
                    {
                        if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
                        {
                            rmq.SetQuorumQueue();
                        }
                    });
                });

                services.AddSingleton<SimulationEffects>();
                services.AddHostedService<ConsoleBackgroundService>();
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.SetWindowSize(65, 15);
        Console.Title = "Processing (Sales)";

        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}