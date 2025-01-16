namespace Shipping;

using System.Threading.Tasks;
using Helper;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class ShipOrderConsumer(SimulationEffects simulationEffects, IHubContext<ShippingHub> shippingHub) : IConsumer<OrderBilled>
{
    public async Task Consume(ConsumeContext<OrderBilled> context)
    {
        await shippingHub.Clients.All.SendAsync("ProcessingOrderBilledMessage", context.Message, context.CancellationToken);
        try
        {
            await simulationEffects.SimulateOrderBilledProcessing(context);
        }
        catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested) { throw; }
        catch
        {
            var messageViewId = DeterministicGuid.GetTheViewId(context.MessageId.ToString(), context.ReceiveContext.InputAddress.ToString());

            await shippingHub.Clients.All.SendAsync("MessageError", context.Message, context.MessageId, messageViewId, context.CancellationToken);
            throw;
        }
    }
}
