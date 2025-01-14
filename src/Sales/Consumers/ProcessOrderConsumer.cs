namespace Sales;

using Messages;
using MassTransit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

public class ProcessOrderConsumer(SimulationEffects simulationEffects, IHubContext<SalesHub> salesHub) : IConsumer<PlaceOrder>
{
    public async Task Consume(ConsumeContext<PlaceOrder> context)
    {
        await salesHub.Clients.All.SendAsync("ProcessingMessage", context.Message, context.CancellationToken);
        // Simulate the time taken to process a message
        await simulationEffects.SimulateMessageProcessing(context);

        var orderPlaced = new OrderPlaced
        {
            OrderId = context.Message.OrderId,
            Contents = context.Message.Contents
        };

        await context.Publish(orderPlaced);
        await salesHub.Clients.All.SendAsync("OrderPlaced", orderPlaced, context.CancellationToken);
    }
}