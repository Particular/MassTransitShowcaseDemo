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
    public static IHostBuilder CreateHostBuilder(string instanceName, string[] args)
    {
        var instanceId = DeterministicGuid.Create("Sales", instanceName);

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

        var instanceName = args.FirstOrDefault();
        if (string.IsNullOrEmpty(instanceName))
        {
            Console.Title = "Processing (Sales)";
            instanceName = "original-instance";
        }
        else
        {
            Console.Title = $"Sales - {instanceName}";
        }

        var host = CreateHostBuilder(instanceName, args).Build();
        await host.StartAsync();

        var state = host.Services.GetService<SimulationEffects>();
        await RunUserInterfaceLoop(state, instanceName);
    }

    static Task RunUserInterfaceLoop(SimulationEffects state, string instanceName)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Sales Endpoint - {instanceName}");
            Console.WriteLine("Press F to process messages faster");
            Console.WriteLine("Press S to process messages slower");

            Console.WriteLine("Press ESC to quit");
            Console.WriteLine();

            state.WriteState(Console.Out);

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
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

static class DeterministicGuid
{
    public static Guid Create(params object[] data)
    {
        // use MD5 hash to get a 16-byte hash of the string
        using var provider = MD5.Create();
        var inputBytes = Encoding.Default.GetBytes(string.Concat(data));
        var hashBytes = provider.ComputeHash(inputBytes);
        // generate a guid from the hash:
        return new Guid(hashBytes);
    }
}