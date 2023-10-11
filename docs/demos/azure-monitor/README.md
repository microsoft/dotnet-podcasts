# Azure Monitor

The .NET Podcast is set up to handle reporting Telemetry data to Azure Monitor when configured. The `AzureMonitor` connection string must be set in the `appsettings.json` configuration file for each environment.

## Prerequisites

- An Azure account with an active subscription. If you don't have an Azure subscription, create a [free account](https://azure.microsoft.com/free/) before you begin.
- Azure CLI has been installed and configured. For instructions to install Azure CLI, follow the [How to install the Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli) guide. For instructions on how to authenticate, follow the [Sign in with Azure CLI](https://learn.microsoft.com/cli/azure/authenticate-azure-cli) guide.

## Create a Microsoft.AppInsights Resource

To collect telemetry, we will need to create a Microsoft.AppInsights resource to report to.

1. Create a new application insights web component with the `{APP_NAME}`, `{LOCATION}`, and `{RESOURCE_GROUP}` arguments replaced for your configuration. A list of valid locations for the `{LOCATION}` can be retrieved by running `az account list-locations`.

   ```azurecli-interactive
   az monitor app-insights component create --app {APP_NAME} --kind web --location {LOCATION} --resource-group {RESOURCE_GROUP} --application-type web
   ```

1. Once the command has executed successfully, retrieve the value from the `connectionString` property.

   ```json
   {
     "connectionString": "InstrumentationKey=XXXXXXXXXX;IngestionEndpoint=https://centralus-0.in.applicationinsights.azure.com/;LiveEndpoint=https://centralus.livediagnostics.monitor.azure.com/"
   }
   ```

## Configure API for Azure Monitor

In the `appsettings.json` (or the file for your environment, such as `appsettings.development.json`), add the connection string value to the `AzureMonitor` property.

```json
{
  "ConnectionStrings": {
    "PodcastDb": "Server=localhost, 5433;Database=Podcast;User Id=sa;Password=Pass@word;Encrypt=False",
    "FeedQueue": "UseDevelopmentStorage=true",
    "AzureMonitor": "InstrumentationKey=XXXXXXXXXX;IngestionEndpoint=XXXXX/;LiveEndpoint=XXXXX"
  }
}
```

## Verification

Once the connection string has been added, the `Program.cs` will look for the `AzureMonitor` property and automatically configure telemetry monitoring. To verify that telemetry is being recorded, sign in to the [Azure Portal](https://portal.azure.com) and verify that Application Insights is receiving data.
