namespace Billing
{
    using Microsoft.AspNetCore.SignalR;

    public class BillingHub(SimulationEffects simulationEffects) : Hub
    {
        public async Task IncreaseFailureRate() => await simulationEffects.IncreaseFailureRate();
        public async Task DecreaseFailureRate() => await simulationEffects.DecreaseFailureRate();

    }
}