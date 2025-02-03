# Running with your own RabbitMQ system

1. Open a terminal and run the following command to shut down the showcase containers:

   ```cmd
   docker compose -p particular-platform-showcase -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env down
   ```

   - All of the containers should be shown as `Stopped` and there should no longer be any containers running under `particular-platform-showcase` in docker

1. Open `src/rabbit.env` file, located in the folder that the showcase is cloned to, in an editor and update the RabbitMQ configuration to point to your own RabbitMQ instance.
   - `CONNECTION_STRING` A special connection string to connect to RabbitMQ, see https://docs.particular.net/servicecontrol/transports#rabbitmq for syntax format.
   - `RABBITMQ_MANAGEMENT_API_URL` The management API URL.
   - `RABBITMQ_MANAGEMENT_API_USERNAME` The management API username.
   - `RABBITMQ_MANAGEMENT_API_PASSWORD` The management API password.
1. Update the list of queues you want to monitor by editing the `src/queues.txt` file, also located in the folder that the showcase is cloned to. RabbitMQ is case-sensitive so make sure the names are exact. e.g. `myqueue_error`.
1. Run the following command to start the required containers with the updated environment settings:

   ```cmd
   docker compose -p particular-platform -f docker-compose-base.yml -f compose-rabbitmq-user.yml --env-file rabbit.env --profile infrastructure up
   ```

   - The containers should all show a status of `Healthy`

## Troubleshooting

-
-
- If you encounter any other issues, ask on [our forum](https://discuss.particular.net/tag/masstransit)
