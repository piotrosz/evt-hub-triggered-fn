
Test EventHubs triggered Azure function

Azure functions runtime versions: isolated vs in-process:

https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-in-process-differences

Azure functions hosting plans:

- Consumption (Serverless) - for event-driven workloads
- Functions Premium - ideal for continuous workloads
- App service plan (aka Dedicated) - colocate web apps and functions or need large SKU

https://learn.microsoft.com/en-us/azure/azure-functions/functions-scale#overview-of-plans

Consumer groups (multiple only in Standard or Premium pricing tiers):

```bash
az eventhubs eventhub consumer-group create -g "evt-hub-test-rg" -n "Consumer1" --eventhub-name "test-evhb" --namespace-name "evthubtest"
```

Deployment template:

```bash
az group create --name "evt-hub-test-rg" --location "Poland Central"

az deployment group create --name ExampleDeployment --resource-group evt-hub-test-rg --template-file <path-to-template> \
--parameters storageAccountType=Standard_GRS \
```
