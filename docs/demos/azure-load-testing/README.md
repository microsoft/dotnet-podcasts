# Azure Load Testing

The .NET Podcast app is set up to automatically run load tests using Azure Load Testing in [GitHub Actions](../../../.github/workflows/podcast-loadtest.yml). The URL of the site to test is set via an environment variable `webapp`. In GitHub Actions, this variable is set using the value of `WEBAPP_NAME` e.g. <https://dotnetpodcasts.azurewebsites.net>

## Prerequisites

- An Azure account with an active subscription. If you don't have an Azure subscription, create a [free account](https://azure.microsoft.com/en-us/free/) before you begin.

- Deploy the dotnet-podcasts application. Follow the detailed guidelines [here](../../deploy-websites-services.md).

## Grant access to Azure Load Testing

To grant access to your Azure Load Testing resource, assign the Load Test Contributor role to the service principal. This role grants the service principal access to create and run load tests with your Azure Load Testing service. Learn more about [managing users and roles in Azure Load Testing](./how-to-assign-roles.md).

1. Retrieve the ID of the service principal object. Replace the name of the Service Principal in the command below. You can reuse the same Service Principal that was used to deploy the dotnet-podcasts application.

    ```azurecli-interactive
    object_id=$(az ad sp list --filter "displayname eq 'podcastsp'" --query "[0].id" -o tsv)
    echo $object_id
    ```

1. Assign the `Load Test Contributor` role to the service principal:

    ```azurecli-interactive
    az role assignment create --assignee $object_id \
        --role "Load Test Contributor" \
        --scope /subscriptions/$subscription \
        --subscription $subscription
    ```

---

## GitHub secrets

If you have deployed the dotnet-podcasts application, you would already have the following secrets wich will be re-used.

- `AZURE_CREDENTIALS`: This will be used to login to Azure.
- `AZURE_RESOURCE_GROUP_NAME`: The resource group for creating the Azure Load Testing resource.
- `WEBAPP_NAME`: This is the web app on which the load test will run.

Add the following GitHub secrets:

- `LOAD_TEST_RESOURCE`: The name of the Azure Load Testing resource.
- `LOAD_TEST_LOCATION`: The location for the Azure Load Testing resource. The load is generated from this region.

That's it! You're all set. Now, let's run the configured workflow to run the load test.

You can manually run the Hub action from the `Actions` tab, click on:
* `Select workflow` -> `Podcast Load Test` -> `Run workflow`.
