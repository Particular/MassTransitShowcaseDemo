# Azure Service Bus

1. Open this azure.env file and provide your namespace
2. Paste the code below to make sure you have your container ready.
   `//your docker stuff here`
3. provide the list of queues you want to monitor by editing the `queues.txt` file. e.g. `myqueue_error`
4. Open a CLI of preference and run these commands:

```cmd
// Your code goes here
 docker compose down
 docker compose up
```
