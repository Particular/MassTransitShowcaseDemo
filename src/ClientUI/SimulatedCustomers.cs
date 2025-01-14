namespace ClientUI;

using MassTransit;
using Messages;
using Microsoft.Extensions.DependencyInjection;

public class SimulatedCustomers(IServiceScopeFactory factory)
{
    public long OrdersPlaced { get; private set; } = 0;

    public async Task PlaceSingleOrder(CancellationToken cancellationToken)
    {
        await using var scope = factory.CreateAsyncScope();

        var placeOrderCommand = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };

        await scope.ServiceProvider.GetRequiredService<IPublishEndpoint>().Publish(placeOrderCommand, cancellationToken);
        OrdersPlaced++;
    }
}