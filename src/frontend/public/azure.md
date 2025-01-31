# Running against your own Azure Service Bus system

##

1. Open the `src/asb.env` file in an editor and update the `CONNECTION_STRING` to point to your own Azure Service Bus namespace.
2. Update the list of queues you want to monitor by editing the `queues.txt` file from the `src` folder. Remember that Azure Service Bus queues are all lowercased.
3. Open a terminal and run these commands:

```cmd
docker compose -p particular-platform-showcase -f docker-compose-base.yml -f compose-azure.yml --env-file asb.env down
docker compose -p particular-platform -f docker-compose-base.yml --env-file asb.env --profile infrastructure up
```
