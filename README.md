# MassTransitShowcaseDemo

A MassTransit version of the [NServiceBus Monitoring Demo](https://github.com/Particular/MonitoringDemo/)

## Launching the Particular Platform

The Particular Service Platform can be pulled down using [the showcase provided `docker-compose.yml`](/src/docker-compose.yml) which uses **RabbitMQ**

```cmd
cd src
docker compose up
```
## Prerequisites 
Allow Visual Studio 2024 "multi-launch" so you can easily select the profile you want to run. RabbitMQ, SQS, Azure Service Bus. 
It can be activated by accessing the Tools menu - Manage preview features- Enable Multi-Project Launch profiles.
After this, you should be able to see a Profile dropdown.
If you select multiple startup projects, it isn't easy to select the profile (which will be the last used profile for each project). Visual Studio 2024 allows switching between multiple "multi-launch" setups.

## Selecting transport
Each project has 3 launch profiles to select Azure Service Bus, AmazonSQS, or RabbitMQ as the transport and reads environment variables to initialize the transport.

The demo requires running the Particular Service Platform for MassTransit as well as a RabbitMQ broker. The whole package can be pulled down using Docker compose and the dockerfile at [~/src/docker-compose.yml](/src/docker-compose.yml)
