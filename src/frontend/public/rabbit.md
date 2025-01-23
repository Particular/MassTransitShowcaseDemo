# RabbitMQ

##

1. Open this rabbit.env file and provide the right ports for your RabbitMQ instance
   - If you have your own instance running in docker:
   - If you have your own standalone RabbitMq instance
2. Paste the code below to make sure you have your container ready.
   `//your docker stuff here`
3. provide the list of queues you want to monitor by editing the `queues.txt` file. RabbitMq is case-sensitive so make sure the names are exact. e.g. `myqueue_error`
4. Open a CLI of preference and run these commands:

```cmd
// Your code goes here
docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env down

docker compose -f docker-compose-base.yml -f compose-rabbitmq.yml --env-file rabbit.env up -d
```
