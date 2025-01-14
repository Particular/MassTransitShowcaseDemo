namespace Sales;

using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<SalesHub> salesHub)
{
    public int MessagesProcessed { get; private set; } = 0;
    public int MessagesErrored { get; private set; } = 0;


    public async Task SimulateMessageProcessing(ConsumeContext<PlaceOrder> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            if (Enum.TryParse(failOn, out EndpointNames endpointName) && endpointName == EndpointNames.Sales)
            {
                MessagesErrored++;
                throw new Exception("BOOM! A failure occurred");
            }

            MessagesProcessed++;
        }
        finally
        {
            await salesHub.Clients.All.SendAsync("MessagesProcessed", MessagesProcessed, MessagesErrored, context.CancellationToken);
        }
    }

}