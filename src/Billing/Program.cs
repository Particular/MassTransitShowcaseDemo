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

        var state = host.Services.GetRequiredService<SimulationEffects>();
        _ = RunUserInterfaceLoop(state);

        await host.RunAsync();
    }

    static async Task RunUserInterfaceLoop(SimulationEffects state)
    {
        while (true)
        {
            Console.Clear();
            await Console.Out.WriteLineAsync("""
                Billing Endpoint
                Press I to increase the simulated failure rate
                Press D to decrease the simulated failure rate
                Press CTRL+C to quit

                """);

            state.WriteState(Console.Out);

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.I:
                    state.IncreaseFailureRate();
                    break;
                case ConsoleKey.D:
                    state.DecreaseFailureRate();
                    break;
            }
        }
    }
}