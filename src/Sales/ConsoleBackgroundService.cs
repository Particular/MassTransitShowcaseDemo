namespace Sales;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

class ConsoleBackgroundService(SimulationEffects state) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        state.RateChanged += async (s, e) => await WriteState();

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
                    await state.IncreaseFailureRate();
                    break;
                case ConsoleKey.D:
                    await state.DecreaseFailureRate();
                    break;
                case ConsoleKey.Q:
                    await state.ProcessMessagesFaster();
                    break;
                case ConsoleKey.S:
                    await state.ProcessMessagesSlower();
                    break;
                case ConsoleKey.R:
                    await state.Reset();
                    break;
            }
        }
    }

    async Task WriteState()
    {
        Console.Clear();
        await Console.Out.WriteAsync("""
                Sales Endpoint:

                - Press I to increase the simulated failure rate
                - Press D to decrease the simulated failure rate
                - Press R to reset simulation
                - Press CTRL+C to quit

                """);

        state.WriteState(Console.Out);
    }
}