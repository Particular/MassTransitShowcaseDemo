namespace Shipping;

using System.Threading.Tasks;
using Helper;
using MassTransit;
using Messages;

public class OrderPlacedConsumer(SimulationEffects simulationEffects) : IConsumer<OrderPlaced>
{
    public Task Consume(ConsumeContext<OrderPlaced> context)
    {
        var delay = simulationEffects.SimulateOrderPlacedMessageProcessing(context.CancellationToken);

        ConsoleHelper.WriteMessageProcessed(context.SentTime ?? DateTime.UtcNow);

        return delay;
    }
}