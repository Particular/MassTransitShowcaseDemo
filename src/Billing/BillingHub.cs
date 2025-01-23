namespace Billing
{
    using Microsoft.AspNetCore.SignalR;

    public class BillingHub(SimulationEffects simulationEffects) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("SyncValues", simulationEffects.MessagesProcessed, simulationEffects.MessagesErrored, simulationEffects.ShouldFailRetries);
            await base.OnConnectedAsync();
        }

        public async Task SetFailRetries(bool shouldFailRetries)
        {
            simulationEffects.ShouldFailRetries = shouldFailRetries;
            await Clients.Caller.SendAsync("SyncValues", simulationEffects.MessagesProcessed, simulationEffects.MessagesErrored, simulationEffects.ShouldFailRetries);
        }
    }
}