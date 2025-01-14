namespace ClientUI
{
    using Microsoft.AspNetCore.SignalR;

    public class ClientHub(SimulatedCustomers simulatedCustomers) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("RateChanged", simulatedCustomers.Rate);
            await base.OnConnectedAsync();
        }
        public async Task IncreaseTraffic() => await simulatedCustomers.IncreaseTraffic();
        public async Task DecreaseTraffic() => await simulatedCustomers.DecreaseTraffic();
    }
}