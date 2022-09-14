# Podcast Approval application using Microsoft Power Apps

Before getting started, ensure that you have access to [Microsoft Power Platform](https://make.powerapps.com/) and latest version of the [Microsoft Power Platform CLI](https://docs.microsoft.com/de-de/power-platform/developer/cli/introduction) installed. 

The solution folder is located in **/src/PowerApps/Solution.zip**. Please download this zip file.
Before building the Power App, we need to import the app solution into our [Power Platform environment](https://docs.microsoft.com/en-us/power-platform/admin/environments-overview):

# Importing our Power Apps solution

Open PowerShell and install the latest version of the PAC CLI:

```PowerShell
pac install latest
```

## Authenticate & Select Environment

Before I can import my solution, I must login to the right Power Platform environment using the PAC CLI via [pac auth](https://docs.microsoft.com/en-us/power-platform/developer/cli/reference/auth):

```PowerShell
pac auth create --url https://org4xxxxxxx.api.crm4.dynamics.com
```

You can get your environment URL from Power Platform Developer resources:
![DevResources](./assets/devresources.jpg)
![DevAPI](./assets/devapi.jpg)


## Import solution file

Next, we can import the solution using the PAC CLI via [pac solution import](https://docs.microsoft.com/en-us/power-platform/developer/cli/reference/solution):

```PowerShell
pac solution import --path C:\Users\name\Downloads\Solution.zip
```

## Open up your Power App

Your solution has been succesfully imported into your selected Power Platform environment. You can now find and edit your Power App:
![powerapp](./assets/powerapp.jpg)


# Next Steps

We now have to integrate our back-end services with our Power App. [Follow the detailed guidelines](https://github.com/user/repo/demos/powerapps/README.md) to connect your web API with your Power App. 