# MassTransitShowcaseDemo

A MassTransit version of the [NServiceBus Monitoring Demo](https://github.com/Particular/MonitoringDemo/)

## Launching the Particular Platform for MassTransit

To launch using **RabbitMQ**:
```cmd
cd src
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env up -d
```

To launch using **Amazon SQS**:

First you need to update the variables in `sqs.env` with all the account details and then
```cmd
cd src
docker compose -f docker-compose-base.yml -f compose-sqs.yml --env-file sqs.env up -d
```

To launch using **Azure ServiceBus**:

First you need to update the variables in `asb.env` with all the account details and then
```cmd
cd src
docker compose -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env up -d
```

## Prerequisites

- Have "Enable Multi-Project Launch profiles" enabled

### Enable Multi-Project Launch profiles

Allow Visual Studio 2024 "multi-launch" so you can easily select the profile you want to run. RabbitMQ, SQS, Azure Service Bus. It can be activated by accessing the Tools menu - Manage preview features- Enable Multi-Project Launch profiles. After this, you should be able to see a Profile dropdown. If you select multiple startup projects, it isn't easy to select the profile (which will be the last used profile for each project). Visual Studio 2024 allows switching between multiple "multi-launch" setups.

## Selecting transport

Each project has 3 launch profiles to select Azure Service Bus, AmazonSQS, or RabbitMQ as the transport and reads environment variables to initialize the transport.

The demo requires running the Particular Service Platform for MassTransit as described above.