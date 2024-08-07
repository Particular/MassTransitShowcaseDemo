namespace Sales;

using Messages;
using MassTransit;
using System.Threading.Tasks;

public class PlaceOrderHandler(SimulationEffects simulationEffects) : IConsumer<PlaceOrder>
{
    public async Task Consume(ConsumeContext<PlaceOrder> context)
    {
        // Simulate the time taken to process a message
        await simulationEffects.SimulateMessageProcessing(context.CancellationToken);

        var orderPlaced = new OrderPlaced
        {
            OrderId = context.Message.OrderId
        };

        await context.Publish(orderPlaced);
    }
}