namespace Shipping
{
    using Microsoft.AspNetCore.SignalR;

    public class ShippingHub(SimulationEffects simulationEffects) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("SyncValues", simulationEffects.OrderPlacedProcessed, simulationEffects.OrderPlacedErrored, simulationEffects.OrderBilledProcessed, simulationEffects.OrderBilledErrored, simulationEffects.ShouldFailRetries);
            await base.OnConnectedAsync();
        }

        public async Task SetFailRetries(bool shouldFailRetries)
        {
            simulationEffects.ShouldFailRetries = shouldFailRetries;
            await Clients.Caller.SendAsync("SyncValues", simulationEffects.OrderPlacedProcessed, simulationEffects.OrderPlacedErrored, simulationEffects.OrderBilledProcessed, simulationEffects.OrderBilledErrored, simulationEffects.ShouldFailRetries);
        }
    }
}