namespace Sales;

using Messages;
using MassTransit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Helper;

public class ProcessOrderConsumer(SimulationEffects simulationEffects, IHubContext<SalesHub> salesHub) : IConsumer<PlaceOrder>
{
    public async Task Consume(ConsumeContext<PlaceOrder> context)
    {
        await salesHub.Clients.All.SendAsync("ProcessingMessage", context.Message, context.CancellationToken);
        try
        {
            await simulationEffects.SimulateSalesProcessing(context);
        }
        catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested) { throw; }
        catch
        {
            var messageViewId = ConsoleHelper.GetTheViewId(context.MessageId.ToString(), context.ReceiveContext.InputAddress.ToString());

            await salesHub.Clients.All.SendAsync("MessageError", context.Message, context.MessageId, messageViewId, context.CancellationToken);
            throw;
        }

        var orderPlaced = new OrderPlaced
        {
            OrderId = context.Message.OrderId,
            Contents = context.Message.Contents
        };

        await context.Publish(orderPlaced);
        await salesHub.Clients.All.SendAsync("OrderPlaced", orderPlaced, context.CancellationToken);
    }
}