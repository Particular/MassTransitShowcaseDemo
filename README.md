# The Particular Platform for MassTransit

This showcase consists of 4 processes hosting MassTransit message producers and consumers that implement a simplified order processing logic from an e-commerce system.

![System Overview](diagram.svg "width=680")


## Launching the Showcase in Docker

The showcase requires a connection to a broker (by default RabbitMQ), [ServiceControl](https://hub.docker.com/r/particular/servicecontrol) container, [ServicePulse](https://hub.docker.com/r/particular/servicepulse) container and the [MassTransit Connector for ServiceControl](https://hub.docker.com/r/particular/servicecontrol-masstransit-connector) container.  
To help getting started we have created a few docker compose files that orchestrate all this setup for you.

Run the docker command below from the `src` folder in a terminal.

```cmd
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env up -d
```

### To run against **Azure Service Bus**

The showcase can also be run using Azure Service Bus rather than RabbitMQ.  
First configure the access to your Azure Service Bus namespace by editing the variables in `src/asb.env`.

```env
CONNECTIONSTRING="Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]"
```

Run docker command below from the `src` folder in a terminal.

```cmd
docker compose -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env up -d
```

## Running from an IDE

> [!WARNING]
> When using Visual Studio, ensure you have the "Enable Multi-Project Launch profiles" setting on. Allow Visual Studio 2022 "multi-launch" so you can easily select the profile you want to run.
>
> It can be activated by accessing the Tools menu -> Manage preview features- Enable Multi-Project Launch profiles.

To start the required infrastructure run the following docker command below from the `src` folder in a terminal.

RabbitMQ
```cmd
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env --profile infrastructure --profile frontend up -d
```
Azure Service Bus
```cmd
docker compose -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env --profile infrastructure --profile frontend up -d
```

After opening the solution (from Visual Studio or Rider), choose one of the run profiles that matches the transport configured previously:

- `RabbitMQ`
- `Azure Service Bus`

Run the solution to start the demo.

## Opening the showcase UI

Navigate to http://localhost:61335/ to see the UI.
