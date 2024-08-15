namespace ClientUI;

using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
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

                    x.AddConfigureEndpointsCallback((name, cfg) =>
                    {
                        if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
                        {
                            rmq.SetQuorumQueue();
                        }
                    });

                    x.AddConsumers(Assembly.GetExecutingAssembly());
                });

                services.AddSingleton<SimulatedCustomers>();
                services.AddHostedService(p => p.GetRequiredService<SimulatedCustomers>());
                services.AddHostedService<ConsoleBackgroundService>();
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Load (ClientUI)";
        Console.SetWindowSize(65, 15);

        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}