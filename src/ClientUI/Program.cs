Console.Title = "Load (ClientUI)";
Console.SetWindowSize(65, 15);

var endpointConfiguration = new EndpointConfiguration("ClientUI");
var transport = endpointConfiguration.UseTransport<LearningTransport>();

endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

endpointConfiguration.UniquelyIdentifyRunningInstance()
    .UsingCustomIdentifier(new Guid("EA3E7D1B-8171-4098-B160-1FEA975CCB2C"))
    .UsingCustomDisplayName("original-instance");

var metrics = endpointConfiguration.EnableMetrics();
metrics.SendMetricDataToServiceControl(
    "Particular.Monitoring",
    TimeSpan.FromMilliseconds(500)
);

var routing = transport.Routing();
routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

var endpointInstance = await Endpoint.Start(endpointConfiguration);

var simulatedCustomers = new SimulatedCustomers(endpointInstance);
var cancellation = new CancellationTokenSource();
var simulatedWork = simulatedCustomers.Run(cancellation.Token);

RunUserInterfaceLoop(simulatedCustomers);

cancellation.Cancel();

await simulatedWork;

await endpointInstance.Stop();

void RunUserInterfaceLoop(SimulatedCustomers simulatedCustomers)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Simulating customers placing orders on a website");
        Console.WriteLine("Press T to toggle High/Low traffic mode");
        Console.WriteLine("Press ESC to quit");
        Console.WriteLine();

        simulatedCustomers.WriteState(Console.Out);

        var input = Console.ReadKey(true);

        switch (input.Key)
        {
            case ConsoleKey.T:
                simulatedCustomers.ToggleTrafficMode();
                break;
            case ConsoleKey.Escape:
                return;
        }
    }
}
