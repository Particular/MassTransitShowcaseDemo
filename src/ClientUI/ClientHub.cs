namespace ClientUI
{
    using Microsoft.AspNetCore.SignalR;

    public class ClientHub(SimulatedCustomers simulatedCustomers) : Hub
    {
        public async Task ClientConnected() => await Clients.Caller.SendAsync("RateChanged", simulatedCustomers.Rate);
        public async Task IncreaseTraffic() => await simulatedCustomers.IncreaseTraffic();
        public async Task DecreaseTraffic() => await simulatedCustomers.DecreaseTraffic();
    }
}