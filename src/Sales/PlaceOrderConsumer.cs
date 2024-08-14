namespace Sales;

using Messages;
using MassTransit;
using System.Threading.Tasks;

public class PlaceOrderConsumer(SimulationEffects simulationEffects) : IConsumer<PlaceOrder>
{
    public async Task Consume(ConsumeContext<PlaceOrder> context)
    {
        // Simulate the time taken to process a message
        await simulationEffects.SimulateMessageProcessing(context.CancellationToken);

        var orderPlaced = new OrderPlaced
        {
            OrderId = context.Message.OrderId
        };

        await context.Publish(orderPlaced);

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