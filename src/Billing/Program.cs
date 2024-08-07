#pragma warning disable IDE0010
namespace Billing;

using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
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

                    x.AddConfigureEndpointsCallback((name, cfg) =>
                    {
                        if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
                        {
                            rmq.SetQuorumQueue();
                        }
                    });

                    x.AddConsumers(Assembly.GetExecutingAssembly());
                });

                services.AddSingleton<SimulationEffects>();
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Failure rate (Billing)";
        Console.SetWindowSize(65, 15);

        var host = CreateHostBuilder(args).Build();
        await host.StartAsync();

        var state = host.Services.GetRequiredService<SimulationEffects>();
        await RunUserInterfaceLoop(state);
    }

    static Task RunUserInterfaceLoop(SimulationEffects state)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Billing Endpoint");
            Console.WriteLine("Press F to increase the simulated failure rate");
            Console.WriteLine("Press S to decrease the simulated failure rate");
            Console.WriteLine("Press ESC to quit");
            Console.WriteLine();

            state.WriteState(Console.Out);

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.F:
                    state.IncreaseFailureRate();
                    break;
                case ConsoleKey.S:
                    state.DecreaseFailureRate();
                    break;
                case ConsoleKey.Escape:
                    return Task.CompletedTask;
            }
        }
    }
}