namespace Shipping;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<ShippingHub> shippingHub)
{
    int orderBilledProcessed = 0;
    int orderBilledErrored = 0;
    int orderPlacedProcessed = 0;
    int orderPlacedErrored = 0;

    public int OrderBilledProcessed { get => orderBilledProcessed; private set => orderBilledProcessed = value; }
    public int OrderBilledErrored { get => orderBilledErrored; private set => orderBilledErrored = value; }
    public int OrderPlacedProcessed { get => orderPlacedProcessed; private set => orderPlacedProcessed = value; }
    public int OrderPlacedErrored { get => orderPlacedErrored; private set => orderPlacedErrored = value; }
    public bool ShouldFailRetries { get; set; } = false;


    public async Task SimulateOrderBilledProcessing(ConsumeContext<OrderBilled> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            //Retries leave ServiceControl headers on the ReceiveContext. Choosing one at random here...
            var isRetry = context.ReceiveContext.TransportHeaders.TryGetHeader("ServiceControl.RetryTo", out var _);
            if (isRetry)
            {
                await shippingHub.Clients.All.SendAsync("RetryAttempted");
            }
            if (Enum.TryParse(failOn, out Consumers endpointName) && endpointName == Consumers.ShippingOrderBilled
                    && (!isRetry || ShouldFailRetries))
            {
                Interlocked.Increment(ref orderBilledErrored);
                throw new OrderBilledException($"A simulated failure occurred in Shipping Order Billed handling, OrderId: {context.Message.OrderId}, Contents: {string.Join(", ", context.Message.Contents)}");
            }

            Interlocked.Increment(ref orderBilledProcessed);
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
            if (isRetry)
            {
                await shippingHub.Clients.All.SendAsync("RetryAttempted");
            }
            if (Enum.TryParse(failOn, out Consumers endpointName) && endpointName == Consumers.ShippingOrderPlaced
                    && (!isRetry || ShouldFailRetries))
            {
                Interlocked.Increment(ref orderPlacedErrored);
                throw new OrderPlacedException($"A simulated failure occurred in Shipping Order Placed handling, OrderId: {context.Message.OrderId}, Contents: {string.Join(", ", context.Message.Contents)}");
            }

            Interlocked.Increment(ref orderPlacedProcessed);
        }
        finally
        {
            await shippingHub.Clients.All.SendAsync("SyncValues", OrderPlacedProcessed, OrderPlacedErrored, OrderBilledProcessed, OrderBilledErrored, ShouldFailRetries, context.CancellationToken);
        }
    }
}