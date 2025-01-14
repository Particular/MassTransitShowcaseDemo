namespace ClientUI
{
    using Microsoft.AspNetCore.SignalR;

    public class ClientHub(SimulatedCustomers simulatedCustomers) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //await Clients.Caller.SendAsync("RateChanged", simulatedCustomers.Rate);
            await base.OnConnectedAsync();
        }

        public async Task CreateOrder()
        {
            await simulatedCustomers.PlaceSingleOrder(Context.ConnectionAborted);
            await Clients.All.SendAsync("OrderPlaced", simulatedCustomers.OrdersPlaced, Context.ConnectionAborted);
        }
    }
}