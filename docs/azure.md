# Running against your own Azure Service Bus system

If you encounter any issues running the steps below, try the [troubleshooting](#troubleshooting) section or ask on [our forum](https://discuss.particular.net/tag/masstransit).

1. Open a terminal and run the following command to shut down the showcase containers:

   ```cmd
   docker compose -p particular-platform-showcase -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env down
   ```

   - All of the containers should be shown as `Stopped` and there should no longer be any containers running under `particular-platform-showcase` in docker

1. Open the `src/asb.env` file, located in the folder that the showcase is cloned to, in an editor and update the `CONNECTION_STRING` to point to your own Azure Service Bus namespace.
1. Update the list of queues you want to monitor by editing the `src/queues.txt` file, also located in the folder that the showcase is cloned to. Remember that Azure Service Bus queue names are all lowercase.
1. Run the following command to start the required containers with the updated environment settings:

   ```cmd
   docker compose -p particular-platform -f docker-compose-base.yml --env-file asb.env --profile infrastructure up
   ```

   - The containers should all show a status of `Healthy`

## Troubleshooting

-
-
