namespace Sales
{
    using Microsoft.AspNetCore.SignalR;

    public class SalesHub(SimulationEffects simulationEffects) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("MessagesProcessed", simulationEffects.MessagesProcessed, simulationEffects.MessagesErrored);
            await base.OnConnectedAsync();
        }
    }
}