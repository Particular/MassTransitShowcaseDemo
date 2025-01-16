namespace Shipping;

using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<ShippingHub> shippingHub)
{
    public int OrderBilledProcessed { get; private set; } = 0;
    public int OrderBilledErrored { get; private set; } = 0;

    public int OrderPlacedProcessed { get; private set; } = 0;
    public int OrderPlacedErrored { get; private set; } = 0;

    public bool ShouldFailRetries { get; set; } = false;


    public async Task SimulateOrderBilledProcessing(ConsumeContext<OrderBilled> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            //Retries leave ServiceControl headers on the ReceiveContext. Choosing one at random here...
            var isRetry = context.ReceiveContext.TransportHeaders.TryGetHeader("ServiceControl.RetryTo", out var _);
            if (Enum.TryParse(failOn, out Consumers endpointName) && endpointName == Consumers.ShippingOrderBilled
                    && (!isRetry || ShouldFailRetries))
            {
                OrderBilledErrored++;
                throw new Exception($"A simulated failure occurred in Shipping Order Billed handling, OrderId: {context.Message.OrderId}, Contents: {string.Join(", ", context.Message.Contents)}");
            }

            OrderBilledProcessed++;
        }
        finally
        {
            await shippingHub.Clients.All.SendAsync("SyncValues", OrderPlacedProcessed, OrderPlacedErrored, OrderBilledProcessed, OrderBilledErrored, ShouldFailRetries, context.CancellationToken);
        }
    }

    public async Task SimulateOrderPlacedProcessing(ConsumeContext<OrderPlaced> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            //Retries leave ServiceControl headers on the ReceiveContext. Choosing one at random here...
            var isRetry = context.ReceiveContext.TransportHeaders.TryGetHeader("ServiceControl.RetryTo", out var _);
            if (Enum.TryParse(failOn, out Consumers endpointName) && endpointName == Consumers.ShippingOrderPlaced
                    && (!isRetry || ShouldFailRetries))
            {
                OrderPlacedErrored++;
                throw new Exception($"A simulated failure occurred in Shipping Order Placed handling, OrderId: {context.Message.OrderId}, Contents: {string.Join(", ", context.Message.Contents)}");
            }

            OrderPlacedProcessed++;
        }
        finally
        {
            await shippingHub.Clients.All.SendAsync("SyncValues", OrderPlacedProcessed, OrderPlacedErrored, OrderBilledProcessed, OrderBilledErrored, ShouldFailRetries, context.CancellationToken);
        }
    }
}