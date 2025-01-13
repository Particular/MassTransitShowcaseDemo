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
        try
        {
            await simulationEffects.SimulatedMessageProcessing(context.CancellationToken);
        }
        catch (OperationCanceledException) when (context.CancellationToken.IsCancellationRequested) { throw; }
        catch (Exception e)
        {
            await billingHub.Clients.All.SendAsync("Exception", e.Message);
            throw;
        }

        var orderBilled = new OrderBilled
        {
            OrderId = context.Message.OrderId
        };

        await context.Publish(orderBilled);

        await ConsoleHelper.WriteMessageProcessed(context.SentTime ?? DateTime.UtcNow);
    }
}