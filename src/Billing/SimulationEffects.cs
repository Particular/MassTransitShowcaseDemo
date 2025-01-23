namespace Billing;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<BillingHub> billingHub)
{
    int messagesErrored = 0;
    int messagesProcessed = 0;

    public int MessagesProcessed { get => messagesProcessed; private set => messagesProcessed = value; }
    public int MessagesErrored { get => messagesErrored; private set => messagesErrored = value; }
    public bool ShouldFailRetries { get; set; } = false;


    public async Task SimulateBillingProcessing(ConsumeContext<OrderPlaced> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            //Retries leave ServiceControl headers on the ReceiveContext. Choosing one at random here...
            var isRetry = context.ReceiveContext.TransportHeaders.TryGetHeader("ServiceControl.RetryTo", out var _);
            if (isRetry)
            {
                await billingHub.Clients.All.SendAsync("RetryAttempted");
            }
            if (Enum.TryParse(failOn, out Consumers endpointName) && endpointName == Consumers.Billing
                    && (!isRetry || ShouldFailRetries))
            {
                Interlocked.Increment(ref messagesErrored);
                throw new BillingProcessingException($"A simulated failure occurred in Billing, OrderId: {context.Message.OrderId}, Contents: {string.Join(", ", context.Message.Contents)}");
            }

            Interlocked.Increment(ref messagesProcessed);
        }
        finally
        {
            await billingHub.Clients.All.SendAsync("SyncValues", MessagesProcessed, MessagesErrored, ShouldFailRetries, context.CancellationToken);
        }
    }
}