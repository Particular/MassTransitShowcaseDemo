namespace Billing;

using System.Threading.Tasks;
using MassTransit;
using Messages;

public class OrderPlacedHandler(SimulationEffects simulationEffects) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        await simulationEffects.SimulatedMessageProcessing(context.CancellationToken);

        var orderBilled = new OrderBilled
        {
            OrderId = context.Message.OrderId
        };

        await context.Publish(orderBilled);
    }
}