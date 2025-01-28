# The Particular Platform for MassTransit

The sample consists of 4 console applications hosting MassTransit message producers and consumers that implement a simplified order processing logic from an e-commerce system.

![System Overview](diagram.svg "width=680")


## Launching the Showcase with Docker

The showcase requires a transport (by default RabbitMQ), ServiceControl, ServicePulse and the MassTransit Connector for ServiceControl. All of these processes are run from a Docker compose file.

Run docker command below from the `src` folder in a CLI

```cmd
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env up -d
```

## **Azure ServiceBus**

The showcase can also be run using ASB rather than RabbitMQ. Configure the access to your Azure Service Bus namespace by editing the variables in `src/asb.env`

```env
CONNECTIONSTRING="Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]"
```

Run docker command below from the `src` folder in a CLI

```cmd
docker compose -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env up -d
```

## Debugging the Showcase

> [!WARNING]
> When using Visual Studio, ensure you have the "Enable Multi-Project Launch profiles" setting on. Allow Visual Studio 2022 "multi-launch" so you can easily select the profile you want to run.
>
> It can be activated by accessing the Tools menu -> Manage preview features- Enable Multi-Project Launch profiles.

> [!NOTE]
> Debugging the showcase still requires a transport, ServiceControl, ServicePulse and the MassTransit Connector for ServiceControl. These can also be debugged locally, or they can be started as part of the Docker compose above, with the showcase process containers stopped/removed.

After opening the solutions (from Visual Studio or Rider), choose one of the run profiles that matches the transport configured previously:

- `RabbitMQ`
- `Azure Service Bus`

Run the solution to start the demo.

## Running the showcase

Navigate to http://localhost:61335/ to see the UI
