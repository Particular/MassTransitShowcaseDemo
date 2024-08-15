#pragma warning disable IDE0010
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
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Load (ClientUI)";
        Console.SetWindowSize(65, 15);

        var host = CreateHostBuilder(args).Build();

        var customers = host.Services.GetRequiredService<SimulatedCustomers>();

        await Task.WhenAny(host.RunAsync(), RunUserInterfaceLoop(customers));
    }

    static async Task RunUserInterfaceLoop(SimulatedCustomers simulatedCustomers)
    {
        while (true)
        {
            Console.Clear();
            await Console.Out.WriteLineAsync("""
                Simulating customers placing orders on a website
                Press T to toggle High/Low traffic mode
                Press CTRL+C to quit

                """);

            simulatedCustomers.WriteState(Console.Out);

            while (!Console.KeyAvailable)
            {
                await Task.Delay(15);
            }

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.T:
                    simulatedCustomers.ToggleTrafficMode();
                    break;
            }
        }
    }
}