namespace ClientUI;

using MassTransit;
using Messages;
using Microsoft.Extensions.Hosting;

class SimulatedCustomers(IBus _bus) : BackgroundService
{
    enum TrafficModes
    {
        Low = 0,
        High = 1,
        Paused = 2
    }

    public void WriteState(TextWriter output)
    {
        output.WriteLine($"{highTrafficMode} traffic mode - sending {rate} orders / second");
    }

    public void ToggleTrafficMode()
    {
        highTrafficMode = (TrafficModes)(((int)highTrafficMode + 1) % 3);
        rate = highTrafficMode switch
        {
            TrafficModes.High => HighTrafficRate,
            TrafficModes.Low => LowTrafficRate,
            TrafficModes.Paused => 0,
            _ => rate
        };
    }

    Task PlaceSingleOrder(CancellationToken cancellationToken)
    {
        var placeOrderCommand = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };

        Console.Write("!");
        return _bus.Publish(placeOrderCommand, cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        nextReset = DateTime.UtcNow.AddSeconds(1);
        currentIntervalCount = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            if (now > nextReset)
            {
                currentIntervalCount = 0;
                nextReset = now.AddSeconds(1);
            }

            if (rate > 0)
            {
                await PlaceSingleOrder(cancellationToken);
            }

            currentIntervalCount++;

            try
            {
                if (currentIntervalCount >= rate)
                {
                    var delay = nextReset - DateTime.UtcNow;
                    if (delay > TimeSpan.Zero)
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    TrafficModes highTrafficMode;

    DateTime nextReset;
    int currentIntervalCount;
    int rate = LowTrafficRate;

    const int HighTrafficRate = 8;
    const int LowTrafficRate = 1;
}