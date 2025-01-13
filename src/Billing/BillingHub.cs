namespace Billing
{
    using Microsoft.AspNetCore.SignalR;

    public class BillingHub(SimulationEffects simulationEffects) : Hub
    {
        public async Task ClientConnected() => await Clients.Caller.SendAsync("FailureRateChanged", Math.Round(simulationEffects.FailureRate * 100, 0));
        public async Task IncreaseFailureRate() => await simulationEffects.IncreaseFailureRate();
        public async Task DecreaseFailureRate() => await simulationEffects.DecreaseFailureRate();

    }
}