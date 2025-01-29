# Running with your own RabbitMQ system

1. Open the `src/rabbit.env` file in an editor and update the RabbitMQ configuration to point to your own RabbitMQ instance.
   1. `CONNECTION_STRING` A special connection string to connect to RabbtiMQ, see https://docs.particular.net/servicecontrol/transports#rabbitmq for syntax format.
   2. `RABBITMQ_MANAGEMENT_API_URL` The management api url.
   3. `RABBITMQ_MANAGEMENT_API_USERNAME` The management api username.
   4. `RABBITMQ_MANAGEMENT_API_PASSWORD` The management api password.
2. Update the list of queues you want to monitor by editing the `src/queues.txt` file. RabbitMQ is case-sensitive so make sure the names are exact. e.g. `myqueue_error`.
3. Open a terminal and run these commands:

```cmd
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env down
docker compose -f docker-compose-base.yml --env-file rabbit.env --profile infrastructure up
```
