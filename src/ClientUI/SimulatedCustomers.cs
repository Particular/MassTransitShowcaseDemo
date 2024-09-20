namespace ClientUI;

using MassTransit;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class SimulatedCustomers(IServiceScopeFactory factory) : BackgroundService
{
    int _rate = 1;

    public void WriteState(TextWriter output)
    {
        output.WriteLine($"Sending {_rate} orders / second");
    }

    public void IncreaseTraffic()
    {
        _rate++;
    }

    public void DecreaseTraffic()
    {
        if (_rate > 0)
        {
            _rate--;
        }
    }

    async Task PlaceSingleOrder(CancellationToken cancellationToken)
    {
        await using var scope = factory.CreateAsyncScope();

        var placeOrderCommand = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };

        Console.Write("!");

        await scope.ServiceProvider.GetRequiredService<IPublishEndpoint>().Publish(placeOrderCommand, cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.WhenAll(
                    SendBatch(cancellationToken),
                    Task.Delay(1000, cancellationToken)
                );
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    async Task SendBatch(CancellationToken cancellationToken)
    {
        int x = _rate;
        if (_rate > 0)
        {
            var tasks = new List<Task>(x);

            for (int i = 0; i < x; i++)
            {
                tasks.Add(PlaceSingleOrder(cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }
}