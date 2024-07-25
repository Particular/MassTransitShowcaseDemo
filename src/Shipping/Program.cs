﻿namespace Shipping;

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

        Console.Title = "Processing (Shipping)";

        var host = CreateHostBuilder(args).Build();
        await host.StartAsync();

        var state = host.Services.GetService<SimulationEffects>();
        await RunUserInterfaceLoop(state);
    }

    static Task RunUserInterfaceLoop(SimulationEffects state)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Shipping Endpoint");
            Console.WriteLine("Press D to toggle resource degradation simulation");
            Console.WriteLine("Press F to process OrderBilled events faster");
            Console.WriteLine("Press S to process OrderBilled events slower");
            Console.WriteLine("Press ESC to quit");
            Console.WriteLine();

            state.WriteState(Console.Out);

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.D:
                    state.ToggleDegradationSimulation();
                    break;
                case ConsoleKey.F:
                    state.ProcessMessagesFaster();
                    break;
                case ConsoleKey.S:
                    state.ProcessMessagesSlower();
                    break;
                case ConsoleKey.Escape:
                    return Task.CompletedTask;
            }
        }
    }
}