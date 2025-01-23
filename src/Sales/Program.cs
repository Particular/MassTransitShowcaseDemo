namespace Sales;

using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(cfg => cfg.SetMinimumLevel(LogLevel.Warning))
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();
                        x.AddConsumers(Assembly.GetExecutingAssembly());
                        x.SetupTransport(args);
                    });

                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowSpecificOrigin",
                            builder => builder.WithOrigins("http://localhost:61335")
                                              .AllowAnyHeader()
                                              .AllowAnyMethod()
                                              .AllowCredentials());
                    });
                    services.AddSignalR(options => { options.EnableDetailedErrors = true; });
                    services.AddSingleton<SimulationEffects>();
                });
                webBuilder.UseUrls($"http://*:{Environment.GetEnvironmentVariable("LISTENING_PORT") ?? "5001"}");
                webBuilder.Configure(app =>
                {
                    app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:61335").AllowCredentials());
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<SalesHub>("/salesHub");
                    });
                });
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
