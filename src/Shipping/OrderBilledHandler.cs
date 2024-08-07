namespace Shipping;

using System.Threading.Tasks;
using MassTransit;
using Messages;

public class OrderBilledHandler(SimulationEffects simulationEffects) : IConsumer<OrderBilled>
{
    public Task Consume(ConsumeContext<OrderBilled> context)
    {
        return simulationEffects.SimulateOrderBilledMessageProcessing(context.CancellationToken);
    }
}