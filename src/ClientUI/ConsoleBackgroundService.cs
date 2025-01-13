namespace ClientUI;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

class ConsoleBackgroundService(SimulatedCustomers simulatedCustomers) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        simulatedCustomers.RateChanged += async (s, e) => await WriteState();

        while (true)
        {
            await WriteState();

            while (!Console.KeyAvailable)
            {
                await Task.Delay(15, stoppingToken);
            }

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.I:
                    await simulatedCustomers.IncreaseTraffic();
                    break;
                case ConsoleKey.D:
                    await simulatedCustomers.DecreaseTraffic();
                    break;
            }
        }
    }

    async Task WriteState()
    {
        Console.Clear();
        await Console.Out.WriteLineAsync("""
                Simulating customers placing orders on a website:

                - Press I to increate order rate
                - Press D to decrease order rate
                - Press CTRL+C to quit

                """);

        simulatedCustomers.WriteState(Console.Out);
    }
}
