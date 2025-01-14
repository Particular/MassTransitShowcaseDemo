namespace Sales
{
    using Microsoft.AspNetCore.SignalR;

    public class SalesHub(SimulationEffects simulationEffects) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("FailureRateChanged", Math.Round(simulationEffects.FailureRate * 100, 0));
            await Clients.Caller.SendAsync("ProcessingTimeChanged", simulationEffects.BaseProcessingTime.TotalSeconds);
            await base.OnConnectedAsync();
        }

        public async Task IncreaseFailureRate() => await simulationEffects.IncreaseFailureRate();
        public async Task DecreaseFailureRate() => await simulationEffects.DecreaseFailureRate();

        public async Task IncreaseProcessingTime() => await simulationEffects.ProcessMessagesFaster();
        public async Task DecreaseProcessingTime() => await simulationEffects.ProcessMessagesSlower();
    }
}