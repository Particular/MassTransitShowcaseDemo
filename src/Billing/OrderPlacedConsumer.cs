﻿namespace Billing;

using System.Threading.Tasks;
using Helper;
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

        ConsoleHelper.WriteMessageProcessed(context.SentTime ?? DateTime.UtcNow);
    }
}