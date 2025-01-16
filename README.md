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

## Running the code

> [!WARNING]
> When using Visual Studio, make sure you have "Enable Multi-Project Launch profiles" setting on. Allow Visual Studio 2024 "multi-launch" so you can easily select the profile you want to run.
>
> It can be activated by accessing the Tools menu -> Manage preview features- Enable Multi-Project Launch profiles.

After opening the solutions (from Visual Studio or Rider), choose one of the run profiles that matches the transport configured previously:

- `RabbitMQ`
- `Azure Service Bus`

Run the solution to start the demo.

## High-level solution overview

The sample consists of 4 console applications hosting MassTransit message producers and consumers that implement a simplified order processing logic from an e-commerce system.

![System Overview](diagram.svg "width=680")
