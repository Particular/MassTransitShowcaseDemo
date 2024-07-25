using Messages;

namespace ClientUI;

class SimulatedCustomers(IEndpointInstance endpointInstance)
{
    public void WriteState(TextWriter output)
    {
        var trafficMode = highTrafficMode ? "High" : "Low";
        output.WriteLine($"{trafficMode} traffic mode - sending {rate} orders / second");
    }

    public void ToggleTrafficMode()
    {
        highTrafficMode = !highTrafficMode;
        rate = highTrafficMode ? 8 : 1;
    }

    public async Task Run(CancellationToken cancellationToken = default)
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

            await PlaceSingleOrder(cancellationToken);
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

    Task PlaceSingleOrder(CancellationToken cancellationToken)
    {
        var placeOrderCommand = new PlaceOrder
        {
            OrderId = Guid.NewGuid().ToString()
        };

        return endpointInstance.Send(placeOrderCommand, cancellationToken);
    }

    bool highTrafficMode;

    DateTime nextReset;
    int currentIntervalCount;
    int rate = 1;
}