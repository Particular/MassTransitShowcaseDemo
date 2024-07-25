namespace Shipping;

using System.Threading.Tasks;
using MassTransit;
using Messages;

public class OrderPlacedHandler(SimulationEffects simulationEffects) : IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        return simulationEffects.SimulateOrderPlacedMessageProcessing(context.CancellationToken);
    }
}