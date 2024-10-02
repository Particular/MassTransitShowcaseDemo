# MassTransitShowcaseDemo

A MassTransit version of the [NServiceBus Monitoring Demo](https://github.com/Particular/MonitoringDemo/)

## Launching the Particular Platform

The Particular Service Platform can be pulled down using [the showcase provided `docker-compose.yml`](/src/docker-compose.yml) which uses **RabbitMQ**

```cmd
cd src
docker compose up
```

## Selecting transport

Each project has 3 launch profiles to select Azure Service Bus, AmazonSQS or RabbitMQ as the transport and reads environment variables to initialize the transport.

If you select multiple startup projects it isn't easy to select the profile (will be the last used profile for each project). Visual Studio 2024 allows switching between multiple "multi-launch" setups. It can be activated here: Environment - Preview Features - Enable Multi-Project Launch profiles
The demo requires running the Particular Service Platform for MassTransit as well as a RabbitMQ broker. The whole package can be pulled down using Docker compose and the dockerfile at [~/src/docker-compose.yml](/src/docker-compose.yml)
