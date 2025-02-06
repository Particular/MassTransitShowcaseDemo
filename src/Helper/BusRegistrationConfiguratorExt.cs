using dotenv.net;
using MassTransit;

public static class BusRegistrationConfiguratorExt
{
    public static void SetupTransport(this IBusRegistrationConfigurator x, string[] args)
    {
        string selectedTransport = Environment.GetEnvironmentVariable("TRANSPORT_TYPE") ?? "RabbitMQ";

        switch (selectedTransport)
        {
            case "AzureServiceBus":
                string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                x.UsingAzureServiceBus((context, cfg) =>
                   {
                       cfg.Host(connectionString);
                       cfg.ConfigureEndpoints(context);
                   });
                break;
            case "RabbitMQ":
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", 5672, "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                });
                break;
            default:
                throw new ArgumentException("No transport is chosen");
        }

        x.AddConfigureEndpointsCallback((name, cfg) =>
        {
            if (cfg is IRabbitMqReceiveEndpointConfigurator rmq)
            {
                rmq.SetQuorumQueue();
            }

            if (cfg is IServiceBusReceiveEndpointConfigurator sb)
            {
                // Uncomment this code to use deadletter
                //sb.ConfigureDeadLetterQueueDeadLetterTransport();
                //sb.ConfigureDeadLetterQueueErrorTransport();
            }
        });
    }
}
