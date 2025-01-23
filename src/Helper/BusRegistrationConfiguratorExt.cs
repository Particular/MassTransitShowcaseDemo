using dotenv.net;
using MassTransit;

public static class BusRegistrationConfiguratorExt
{
    public static void SetupTransport(this IBusRegistrationConfigurator x, string[] args)
    {
        string selectedTransport = Environment.GetEnvironmentVariable("TRANSPORT_TYPE") ?? "RabbitMQ";

        switch (selectedTransport)
        {
            case "AmazonSQS":
                x.UsingAmazonSqs((ctx, cfg) =>
                {
                    var envs = DotEnv.Read(new DotEnvOptions(envFilePaths: [Path.GetFullPath("../../../sqs.env")]));
                    cfg.Host(envs["AWS_REGION"], h =>
                    {
                        h.AccessKey(envs["AWS_ACCESS_KEY_ID"]);
                        h.SecretKey(envs["AWS_SECRET_ACCESS_KEY"]);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
                break;
            case "AzureServiceBus":
                var envs = DotEnv.Read(new DotEnvOptions(envFilePaths: [Path.GetFullPath("../../../asb.env")], ignoreExceptions: false));
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(envs["CONNECTION_STRING"]);

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
