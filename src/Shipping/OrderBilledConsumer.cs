namespace Shipping;

using System.Threading.Tasks;
using Helper;
using MassTransit;
using Messages;

public class OrderBilledHandler(SimulationEffects simulationEffects) : IConsumer<OrderBilled>
{
    public Task Consume(ConsumeContext<OrderBilled> context)
    {
        var delay = simulationEffects.SimulateOrderBilledMessageProcessing(context.CancellationToken);

        ConsoleHelper.WriteMessageProcessed(context.SentTime ?? DateTime.UtcNow);

        return delay;
    }
}