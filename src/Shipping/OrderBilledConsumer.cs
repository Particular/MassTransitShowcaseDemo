namespace Shipping;

using System.Threading.Tasks;
using MassTransit;
using Messages;

public class OrderBilledHandler(SimulationEffects simulationEffects) : IConsumer<OrderBilled>
{
    public Task Consume(ConsumeContext<OrderBilled> context)
    {
        var delay = simulationEffects.SimulateOrderBilledMessageProcessing(context.CancellationToken);

        if (DateTime.UtcNow - context.SentTime >= TimeSpan.FromSeconds(10))
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.Write(".");

        return delay;
    }
}