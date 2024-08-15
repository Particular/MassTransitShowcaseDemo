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
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.SetWindowSize(65, 15);
        Console.Title = "Processing (Sales)";

        var host = CreateHostBuilder(args).Build();

        var state = host.Services.GetRequiredService<SimulationEffects>();

        await Task.WhenAny(host.RunAsync(), RunUserInterfaceLoop(state));
    }

    static async Task RunUserInterfaceLoop(SimulationEffects state)
    {
        while (true)
        {
            Console.Clear();
            await Console.Out.WriteAsync("""
                Sales Endpoint
                Press Q to process messages faster
                Press S to process messages slower
                Press I to increase the simulated failure rate
                Press D to decrease the simulated failure rate
                Press CTRL+C to quit

                """);

            state.WriteState(Console.Out);

            while (!Console.KeyAvailable)
            {
                await Task.Delay(15);
            }

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.I:
                    state.IncreaseFailureRate();
                    break;
                case ConsoleKey.D:
                    state.DecreaseFailureRate();
                    break;
                case ConsoleKey.F:
                    state.ProcessMessagesFaster();
                    break;
                case ConsoleKey.S:
                    state.ProcessMessagesSlower();
                    break;
            }
        }
    }
}