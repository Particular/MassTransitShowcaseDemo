namespace Billing;

using System.Threading.Tasks;
using Helper;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class BillOrderConsumer(SimulationEffects simulationEffects, IHubContext<BillingHub> billingHub) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        await billingHub.Clients.All.SendAsync("ProcessingMessage", context.Message, context.CancellationToken);
        try
        {
            await simulationEffects.SimulateBillingProcessing(context);
        }
        catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested) { throw; }
        catch
        {
            var messageViewId = DeterministicGuid.GetTheViewId(context.MessageId.ToString(), context.ReceiveContext.InputAddress.ToString());

            await billingHub.Clients.All.SendAsync("MessageError", context.Message, context.MessageId, messageViewId, context.CancellationToken);
            throw;
        }

        var orderBilled = new OrderBilled
        {
            OrderId = context.Message.OrderId,
            Contents = context.Message.Contents
        };

        await context.Publish(orderBilled);
        await billingHub.Clients.All.SendAsync("OrderBilled", orderBilled, context.CancellationToken);
    }
}