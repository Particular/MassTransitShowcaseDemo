namespace ClientUI
{
    using Messages;
    using Microsoft.AspNetCore.SignalR;

    public class ClientHub(SimulatedCustomers simulatedCustomers) : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Initialise", simulatedCustomers.OrdersPlaced, Enum.GetValues<EndpointNames>().Select(x => x.ToString()));
            await base.OnConnectedAsync();
        }

        public async Task CreateOrder(float requestCount, string failOn)
        {
            for (int i = 0; i < (int)requestCount; i++)
            {
                var order = await simulatedCustomers.PlaceSingleOrder(failOn, Context.ConnectionAborted);
                await Clients.All.SendAsync("OrderRequested", order, simulatedCustomers.OrdersPlaced, Context.ConnectionAborted);
            }
        }
    }

}