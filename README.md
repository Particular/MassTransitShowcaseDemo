# The Particular Platform for MassTransit

## Launching the Showcase

To run the code sample you have 2 options in terms of transports:

## **RabbitMQ**

> [!NOTE]
> The RabbitMQ Broker is started as part of the docker compose process

Run docker command below from the `src` folder in a CLI

```cmd
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env up -d
```

## **Azure ServiceBus**

Configure the access to your Azure Service Bus namespace by editing the variables in `src/asb.env`

```env
CONNECTIONSTRING="Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]"
```

Run docker command below from the `src` folder in a CLI

```cmd
docker compose -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env up -d
```

## [Running the Sample](docs.md#Running-the-sample)

## [Read more about how it works](docs.md#Walkthrough)
