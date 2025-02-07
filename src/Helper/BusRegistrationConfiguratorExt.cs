using dotenv.net;
using MassTransit;

public static class BusRegistrationConfiguratorExt
{
    public static void SetupTransport(this IBusRegistrationConfigurator x, string[] args)
    {
        var selectedTransport = Environment.GetEnvironmentVariable("TRANSPORT_TYPE") ?? "RabbitMQ";
        string envFile;

        switch (selectedTransport)
        {
            case "AmazonSQS":
                var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
                var awsClientId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
                var awsSecret = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
                envFile = Path.GetFullPath("../../../sqs.env");
                if (File.Exists(envFile))
                {
                    var envs = DotEnv.Read(new DotEnvOptions(envFilePaths: [envFile]));
                    awsRegion = envs["AWS_REGION"];
                    awsClientId = envs["AWS_ACCESS_KEY_ID"];
                    awsSecret = envs["AWS_SECRET_ACCESS_KEY"];
                }

                x.UsingAmazonSqs((ctx, cfg) =>
                {
                    cfg.Host(awsRegion, h =>
                    {
                        h.AccessKey(awsClientId);
                        h.SecretKey(awsSecret);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
                break;
            case "AzureServiceBus":
                var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
                envFile = Path.GetFullPath("../../../asb.env");
                if (File.Exists(envFile))
                {
                    var envs = DotEnv.Read(new DotEnvOptions(envFilePaths: [envFile], ignoreExceptions: false));
                    connectionString = envs["CONNECTION_STRING"];
                }

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(connectionString);

                    cfg.ConfigureEndpoints(context);
                });

                break;
            case "RabbitMQ":
                var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST");
                var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
                var vHost = Environment.GetEnvironmentVariable("RABBITMQ_VIRTUALHOST");
                var username = Environment.GetEnvironmentVariable("RABBITMQ_MANAGEMENT_API_USERNAME");
                var password = Environment.GetEnvironmentVariable("RABBITMQ_MANAGEMENT_API_PASSWORD");
                envFile = Path.GetFullPath("../../../rabbit.env");
                if (File.Exists(envFile))
                {
                    var envs = DotEnv.Read(new DotEnvOptions(envFilePaths: [envFile], ignoreExceptions: false));
                    host = envs["RABBITMQ_HOST"];
                    port = envs["RABBITMQ_PORT"];
                    vHost = envs["RABBITMQ_VIRTUALHOST"];
                    username = envs["RABBITMQ_MANAGEMENT_API_USERNAME"];
                    password = envs["RABBITMQ_MANAGEMENT_API_PASSWORD"];
                }

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, ushort.Parse(port), vHost, h =>
                    {
                        h.Username(username);
                        h.Password(password);
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
