namespace Billing;

using System.Threading.Tasks;
using MassTransit;
using Messages;

public class OrderPlacedConsumer(SimulationEffects simulationEffects) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        await simulationEffects.SimulatedMessageProcessing(context.CancellationToken);

        var orderBilled = new OrderBilled
        {
            OrderId = context.Message.OrderId
        };

        await context.Publish(orderBilled);

        if (DateTime.UtcNow - context.SentTime >= TimeSpan.FromSeconds(10))
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.Write(".");
    }
}