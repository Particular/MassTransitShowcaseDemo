namespace Shipping;

using System.Threading.Tasks;
using Helper;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class PrepareOrderConsumer(SimulationEffects simulationEffects, IHubContext<ShippingHub> shippingHub) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        await shippingHub.Clients.All.SendAsync("ProcessingOrderPlacedMessage", context.Message, context.CancellationToken);
        try
        {
            await simulationEffects.SimulateOrderPlacedProcessing(context);
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