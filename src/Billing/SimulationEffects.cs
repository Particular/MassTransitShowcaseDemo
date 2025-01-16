﻿namespace Billing;

using MassTransit;
using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<BillingHub> billingHub)
{
    public int MessagesProcessed { get; private set; } = 0;
    public int MessagesErrored { get; private set; } = 0;

    public bool ShouldFailRetries { get; set; } = false;


    public async Task SimulateBillingProcessing(ConsumeContext<OrderPlaced> context)
    {
        try
        {
            context.TryGetHeader("FailOn", out string failOn);
            //Retries leave ServiceControl headers on the ReceiveContext. Choosing one at random here...
            var isRetry = context.ReceiveContext.TransportHeaders.TryGetHeader("ServiceControl.RetryTo", out var _);
            if (Enum.TryParse(failOn, out Consumers endpointName) && endpointName == Consumers.Billing
                    && (!isRetry || ShouldFailRetries))
            {
                MessagesErrored++;
                throw new Exception($"A simulated failure occurred in Billing, OrderId: {context.Message.OrderId}, Contents: {string.Join(", ", context.Message.Contents)}");
            }

            MessagesProcessed++;
        }
        finally
        {
            await billingHub.Clients.All.SendAsync("SyncValues", MessagesProcessed, MessagesErrored, ShouldFailRetries, context.CancellationToken);
        }
    }
}