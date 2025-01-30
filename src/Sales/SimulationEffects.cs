namespace Sales;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<SalesHub> salesHub)
{
    int messagesProcessed = 0;
    int messagesErrored = 0;

    public int MessagesProcessed { get => messagesProcessed; private set => messagesProcessed = value; }
    public int MessagesErrored { get => messagesErrored; private set => messagesErrored = value; }
    public bool ShouldFailRetries { get; set; } = false;


    public async Task SimulateSalesProcessing(ConsumeContext<PlaceOrder> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            //Retries leave ServiceControl headers on the ReceiveContext. Choosing one at random here...
            var isRetry = context.ReceiveContext.TransportHeaders.TryGetHeader("ServiceControl.RetryTo", out var _);
            if (isRetry)
            {
                await salesHub.Clients.All.SendAsync("RetryAttempted");
            }
            if (Enum.TryParse(failOn, out Consumers endpointName) && endpointName == Consumers.Sales
                    && (!isRetry || ShouldFailRetries))
            {
                Interlocked.Increment(ref messagesErrored);
                throw new SalesProcessingException($"A simulated failure occurred in Sales, OrderId: {context.Message.OrderId}, Contents: {string.Join(", ", context.Message.Contents)}");
            }
            else if (isRetry)
            {
                await salesHub.Clients.All.SendAsync("RetrySuccessful", context.Message.OrderId);
            }

            Interlocked.Increment(ref messagesProcessed);
        }
        finally
        {
            await salesHub.Clients.All.SendAsync("SyncValues", MessagesProcessed, MessagesErrored, ShouldFailRetries, context.CancellationToken);
        }
    }

}