namespace ClientUI;

using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class SimulatedCustomers(IServiceScopeFactory factory, IHubContext<ClientHub> clientHub) : BackgroundService
{
    long ordersPlaced = 0;

    public int Rate { get; private set; } = 1;

    public event EventHandler RateChanged;

    public void WriteState(TextWriter output)
    {
        output.WriteLine($"Sending {Rate} orders / second");
    }

    public async Task IncreaseTraffic()
    {
        Rate++;
        await NotifyOfRateChange();
    }

    public async Task DecreaseTraffic()
    {
        if (Rate > 0)
        {
            Rate--;
            await NotifyOfRateChange();
        }
    }

    async Task PlaceSingleOrder(CancellationToken cancellationToken)
    {
        await using var scope = factory.CreateAsyncScope();

        var placeOrderCommand = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };

        Console.Write("!");

        await scope.ServiceProvider.GetRequiredService<IPublishEndpoint>().Publish(placeOrderCommand, cancellationToken);
        ordersPlaced++;
        await clientHub.Clients.All.SendAsync("OrderPlaced", ordersPlaced, cancellationToken);
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
        int x = Rate;
        if (Rate > 0)
        {
            var tasks = new List<Task>(x);

            for (int i = 0; i < x; i++)
            {
                tasks.Add(PlaceSingleOrder(cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }

    async Task NotifyOfRateChange()
    {
        await clientHub.Clients.All.SendAsync("RateChanged", Rate);
        RateChanged?.Invoke(this, EventArgs.Empty);
    }
}