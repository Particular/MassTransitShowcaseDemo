namespace ClientUI
{
    using Microsoft.AspNetCore.SignalR;

    public class ClientHub(SimulatedCustomers simulatedCustomers) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("OrderPlaced", null, simulatedCustomers.OrdersPlaced);
            await base.OnConnectedAsync();
        }

        public async Task CreateOrder()
        {
            var order = await simulatedCustomers.PlaceSingleOrder(Context.ConnectionAborted);
            await Clients.All.SendAsync("OrderPlaced", order, simulatedCustomers.OrdersPlaced, Context.ConnectionAborted);
        }
    }

}