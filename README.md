# MassTransitShowcaseDemo

A MassTransit version of the [NServiceBus Monitoring Demo](https://github.com/Particular/MonitoringDemo/)

The Particular Service Platform can be pulled down using [this Docker compose file](/src/docker-compose.yml).

## Selecting transport

Each project has 3 launch profiles to select Azure Service Bus, AmazonSQS or RabbitMQ as the transport and reads environment variables to initialize the transport.

If you select multiple startup projects it isn't easy to select the profile (will be the last used profile for each project). Visual Studio 2024 allows switching between multiple "multi-launch" setups. It can be activated here: Environment - Preview Features - Enable Multi-Project Launch profiles
