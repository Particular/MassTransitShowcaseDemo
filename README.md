# MassTransitShowcaseDemo


## Launching the Particular Platform for MassTransit
To run the code sample you have 3 options in terms of transports:

## **RabbitMQ**

>[!NOTE]
The RabbitMQ Broker is started as part of the docker compose process
>

Run docker command bellow from the `src` folder in a CLI
```cmd
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env up -d
```
## **Azure ServiceBus**

Configure the access to your Azure Service Bus namespace by editing the variables in `asb.env`  

```cmd
CONNECTIONSTRING="Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]"
```

Set the same environment variables on your machine:

```cmd
setx CONNECTIONSTRING_AZURESERVICEBUS "Endpoint=sb://[NAMESPACE].servicebus.windows.net/;SharedAccessKeyName=[KEYNAME];SharedAccessKey=[KEY]"
```

Run docker command bellow from the `src` folder in a CLI

```cmd
docker compose -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env up -d
```

## **Amazon SQS**

Configure the access to your SQS namespace by editing the variables in `sqs.env`  

```cmd
AWS_REGION="<region>"
AWS_ACCESS_KEY_ID="<access-key>"
AWS_SECRET_ACCESS_KEY="<secret-access-key>"
```

Set the same environment variables on your machine:

```cmd
setx AWS_REGION "<region>"
setx AWS_ACCESS_KEY_ID "<access-key>"
setx AWS_SECRET_ACCESS_KEY "<secret-access-key>"
```

Run docker command bellow from the `src` folder in a CLI

```cmd
docker compose -f docker-compose-base.yml -f compose-sqs.yml --env-file sqs.env up -d
```

## Running the code


>[!WARNING]
> When using Visual Studio, make sure you have "Enable Multi-Project Launch profiles" setting on. Allow Visual Studio 2024 "multi-launch" so you can easily select the profile you want to run.
>
> It can be activated by accessing the Tools menu -> Manage preview features- Enable Multi-Project Launch profiles.
>

After opening the solutions (from Visual Studio or Rider), choose one of the run profiles  that matches the transport configured previously:
- `RabbitMQ`
- `AmazonSQS`
- `Azure Service Bus`

Run the solution to start the demo.