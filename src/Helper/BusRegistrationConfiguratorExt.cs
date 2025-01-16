using dotenv.net;
using Helper;
using MassTransit;

public static class BusRegistrationConfiguratorExt
{
    public static void SetupTransport(this IBusRegistrationConfigurator x, string[] args)
    {
        string selectedTransport;

        if (args.Contains("--amazonsqs"))
        {
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
            selectedTransport = "AmazonSQS";
        }
        else if (args.Contains("--azureservicebus"))
        {
            var envs = DotEnv.Read(new DotEnvOptions(envFilePaths: [Path.GetFullPath("../../../asb.env")], ignoreExceptions: false));
            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(envs["CONNECTIONSTRING"]);

                cfg.ConfigureEndpoints(context);
            });

            selectedTransport = "Azure Service Bus";
        }
        else if (args.Contains("--rabbitmq"))
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", 56721, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context);
            });
            selectedTransport = "RabbitMQ";
        }
        else
        {
            throw new ArgumentException("No transport is chosen");
        }

        var name = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
        Console.Title = name + " - " + selectedTransport;

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
