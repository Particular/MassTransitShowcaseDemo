namespace Sales;

using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<SalesHub> salesHub)
{
    public int MessagesProcessed { get; private set; } = 0;
    public int MessagesErrored { get; private set; } = 0;

    public bool ShouldFailRetries { get; set; } = false;


    public async Task SimulateMessageProcessing(ConsumeContext<PlaceOrder> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            //Retries leave ServiceControl headers on the ReceiveContext. Choosing one at random here...
            var isRetry = context.ReceiveContext.TransportHeaders.TryGetHeader("ServiceControl.RetryTo", out var _);
            if (Enum.TryParse(failOn, out EndpointNames endpointName) && endpointName == EndpointNames.Sales
                    && (!isRetry || ShouldFailRetries))
            {
                MessagesErrored++;
                throw new Exception($"A simulated failure occurred in Sales");
            }

            MessagesProcessed++;
        }
        finally
        {
            await salesHub.Clients.All.SendAsync("SyncValues", MessagesProcessed, MessagesErrored, ShouldFailRetries, context.CancellationToken);
        }
    }

}