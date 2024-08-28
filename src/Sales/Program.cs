#pragma warning disable IDE0010
namespace Sales;

using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Logging;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(cfg => cfg.ClearProviders())
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.UsingAmazonSqs((ctx, cfg) =>
                    {
                        cfg.Host(Environment.GetEnvironmentVariable("AWS_REGION"), h =>
                        {
                            h.AccessKey(Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"));
                            h.SecretKey(Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"));
                        });

                        cfg.ConfigureEndpoints(ctx);
                    });

                    x.AddConsumers(Assembly.GetExecutingAssembly());
                });

                services.AddSingleton<SimulationEffects>();
                services.AddHostedService<ConsoleBackgroundService>();
            });

        return host;
    }

    static async Task Main(string[] args)
    {
        Console.Title = "Processing (Sales)";

        var host = CreateHostBuilder(args).Build();
        await host.RunAsync();
    }
}